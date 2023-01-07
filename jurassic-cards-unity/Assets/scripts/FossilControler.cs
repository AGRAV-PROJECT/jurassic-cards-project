using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FossilController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";

    // Update is called once per frame
    void Update()
    {

    }

    // Check if there is a signed in user. If so, he should be redirected to the home page
    void Start()
    {
        if (PlayerPrefs.GetInt("Current_Logged_UserID", 0) == 0)
        {
            Debug.Log("No logged in user");
        }
        else
        {
            SceneManager.LoadScene(8);
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

        // Call Get Fossil Info Endpoint
        string uri = API_URI + "fossil/check?userID=" + userID;
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
                    Fossil fossil = JsonUtility.FromJson<Fossil>(webRequest.downloadHandler.text);

                    Debug.Log(fossil.ToString());

                    webRequest.Dispose();
                    break;
            }
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
