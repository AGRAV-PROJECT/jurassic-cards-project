using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FossilController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";
    public Transform content;
    public GameObject fossilPrefab;
    public Sprite plesioSprite;
    public Sprite trxSprite;
    public GoogleMaps googleMaps;
    public int selectedFossilID;

    private float timeRemaining = 5;

    private void Update()
    {
        if (timeRemaining > 0)
        {
            //if the countdown have yet to reach 0 the timer will continue to countdown

            timeRemaining -= Time.deltaTime;
        }
        else
        {
            //when the timer reaches 0 the game will look for fossils
            StartCoroutine(FindFossils());
            timeRemaining = 5;
        }
    }

    // Fossil
    public class Fossil
    {

        public string playerID;
        public string name;
        public string description;
        public string image;
        public string date;
        public double latitude;
        public double longitude;
        public bool isPlanted;

        public string toString()
        {
            return name + "-" + description + "-" +
            image + "-" + date + "-" + latitude + "-" + longitude +
            "-" + isPlanted;
            ;
        }
    }

    public void PlantFossil()
    {
        StartCoroutine(PlantFossilCoroutine(selectedFossilID));
    }

    IEnumerator PlantFossilCoroutine(int fossilID)
    {
        // Get coordinates
        double latitude = googleMaps.lat;
        double longitude = googleMaps.lon;

        // Create fossil class
        var fossil = new Fossil();
        fossil.latitude = latitude;
        fossil.longitude = longitude;

        // Create JSON from class
        string json = JsonUtility.ToJson(fossil);

        // Create web request
        var request = new UnityWebRequest(API_URI + "fossil/plant?iD=" + fossilID, "PUT");

        // Encode JSON to send in the request and change content type on request header accordingly
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
            request.Dispose();
        }
        else
        {
            Debug.Log("Fossil planted successfully!");
            request.Dispose();
        }
    }

    IEnumerator FindFossils()
    {
        //Get UserID
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);

        string requestResult = "";

        // Get coordinates
        double latitude = googleMaps.lat;
        double longitude = googleMaps.lon;

        // Create fossil class
        var fossil = new Fossil();
        fossil.latitude = latitude;
        fossil.longitude = longitude;

        // Create JSON from class
        string json = JsonUtility.ToJson(fossil);

        // Create web request
        var request = new UnityWebRequest(API_URI + "fossil/find?userID=" + userID, "GET");

        // Encode JSON to send in the request and change content type on request header accordingly
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
            request.Dispose();
        }
        else
        {
            request.Dispose();

            // Take care of JSON
            JsonFossilList FossilList = new JsonFossilList();
            FossilList = JsonUtility.FromJson<JsonFossilList>("{\"fossilList\":" + requestResult.ToString() + "}");

            if (FossilList.fossilList.Count > 0)
            {
                JsonFossil fossilResult = FossilList.fossilList[0];
                Debug.Log("Fossil found - id: " + fossilResult.id);
            }
            else
            {
                Debug.Log("No fossils found near player");
            }
        }
    }

    // Call Get Fossil Info IEnumerator
    public void GetFossilInfo()
    {
        StartCoroutine(GetFossilInfoCoroutine());
    }

    // Get Fossil Info logic
    IEnumerator GetFossilInfoCoroutine()
    {
        //Get FossilID
        int iD = 1; //Get from the fossil selected 

        // Call Get Fossil Info Endpoint
        string uri = API_URI + "fossil/check/" + iD;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {


            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    Fossil fossil = JsonUtility.FromJson<Fossil>(webRequest.downloadHandler.text);

                    Debug.Log(fossil.ToString());

                    webRequest.Dispose();
                    break;
            }
        }
    }

    // Cards list
    [Serializable]
    public class JsonFossilList
    {
        public List<JsonFossil> fossilList;
    }

    // JSON Card class
    [Serializable]
    public class JsonFossil
    {
        public int id;
        public string name;
        public string image;
    }

    // Call Get Fossil by Player IEnumerator
    public void GetFossilbyPlayer()
    {
        StartCoroutine(GetFossilbyPlayerCoroutine());
    }

    // Get Fossil by Player logic
    IEnumerator GetFossilbyPlayerCoroutine()
    {

        //Get UserID
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);

        string requestResult = "";

        // Call Get Fossil Info Endpoint
        string uri = API_URI + "/fossil/collection?userID=" + userID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    Debug.Log(webRequest.downloadHandler.text);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    requestResult = webRequest.downloadHandler.text;

                    webRequest.Dispose();
                    break;
            }
        }

        // Take care of JSON
        JsonFossilList FossilList = new JsonFossilList();
        FossilList = JsonUtility.FromJson<JsonFossilList>("{\"fossilList\":" + requestResult.ToString() + "}");

        foreach (JsonFossil item in FossilList.fossilList)
        {
            if (item.image.Contains("Plesiossauros"))
            {
                GameObject obj = Instantiate(fossilPrefab, content);
                var itemName = obj.transform.Find("CardText").GetComponent<Text>();
                var itemIcon = obj.transform.Find("CardImage").GetComponent<Image>();

                fossilPrefab.GetComponent<FossilPlantSelection>().fossilID = item.id;
                itemName.text = "Plesiosauro";
                itemIcon.sprite = plesioSprite;
            }
            else if (item.image.Contains("Triceratops"))
            {

            }
            else if (item.image.Contains("TRex"))
            {
                GameObject obj = Instantiate(fossilPrefab, content);
                var itemName = obj.transform.Find("CardText").GetComponent<Text>();
                var itemIcon = obj.transform.Find("CardImage").GetComponent<Image>();

                fossilPrefab.GetComponent<FossilPlantSelection>().fossilID = item.id;
                itemName.text = "T-Rex";
                itemIcon.sprite = trxSprite;
            }
            else if (item.image.Contains("Spinosauros"))
            {

            }
            else
            {
                GameObject obj = Instantiate(fossilPrefab, content);
                var itemName = obj.transform.Find("CardText").GetComponent<Text>();
                var itemIcon = obj.transform.Find("CardImage").GetComponent<Image>();

                fossilPrefab.GetComponent<FossilPlantSelection>().fossilID = item.id;
                itemName.text = "Fossil";
                itemIcon.sprite = trxSprite;
            }
        }
    }

    private void OnEnable()
    {
        GetFossilbyPlayer();
    }

    private void OnDisable()
    {
        while (content.childCount > 0)
        {
            DestroyImmediate(content.GetChild(0).gameObject);
        }
    }

    // Call Add Fossil IEnumerator
    public void AddFossil()
    {
        StartCoroutine(AddFossilCoroutine());
    }

    // Add Fossil Card
    IEnumerator AddFossilCoroutine()
    {


        //Get UserID
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);

        // Create fossil class
        var fossil = new Fossil();

        //Get values from player inputs
        /* 
        fossil.playerID= playerID;
        fossil.name= name;
        fossil.description= description;
        fossil.image= image;
        fossil.date= date;
        fossil.latitude= latitude;
        fossil.longitude= longitude;
        fossil.isPlanted= isPlanted; */


        // Call Add Fossil Endpoint
        string uri = API_URI + "fossil/add?userID=" + userID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            // Create JSON from class
            string json = JsonUtility.ToJson(fossil);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    //Display success of the operation to the user
                    webRequest.Dispose();
                    break;
            }
        }
    }

    // Call Update Fossil IEnumerator
    public void UpdateFossil()
    {
        StartCoroutine(UpdateFossilCoroutine());
    }

    // Update Fossil Card
    IEnumerator UpdateFossilCoroutine()
    {
        //Get fossilID
        int fossilID = 1; // REPLACE with the id of the fossil that the player chooses to update

        // Call Update Fossil Endpoint
        string uri = API_URI + "fossil/plant?iD=" + fossilID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    //Display success of the operation to the user
                    webRequest.Dispose();
                    break;
            }
        }
    }


    // Call Delete Fossil IEnumerator
    public void DeleteFossil()
    {
        StartCoroutine(DeleteFossilCoroutine());
    }

    // Delete Fossil IEnumerator logic
    IEnumerator DeleteFossilCoroutine()
    {

        //Get FossilID
        int fossilID = 5; // REPLACE HERE, fossil that the user clickes/chooses to delete


        // Call Delete Fossil Endpoint
        string uri = API_URI + "fossil/discard?fossilID=" + fossilID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();


            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    //Display success of the operation to the user
                    // 
                    webRequest.Dispose();
                    break;
            }
        }
    }

}
