using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FossilFinderController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";
    [SerializeField] HomePageController homePageController;
    [SerializeField] GameObject fossilFoundPopup;
    public GoogleMaps googleMaps;
    private int fossilIDFound;
    private float timeRemaining = 5;

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

    public class Fossil2
    {
        public int iD;
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
            if (!homePageController.CheckIfMenuOpen())
            {
                StartCoroutine(FindFossils());
                timeRemaining = 5;
            }
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
            requestResult = request.downloadHandler.text;
            request.Dispose();

            // Take care of JSON
            JsonFossilList FossilList = new JsonFossilList();
            Debug.Log(requestResult.ToString());
            FossilList = JsonUtility.FromJson<JsonFossilList>("{\"fossilList\":" + requestResult.ToString() + "}");

            if (FossilList.fossilList.Count > 0)
            {
                JsonFossil fossilResult = FossilList.fossilList[0];
                Debug.Log("Fossil found - id: " + fossilResult.id);

                fossilIDFound = fossilResult.id;
                if (!homePageController.CheckIfMenuOpen())
                {
                    homePageController.OpenMenuWithoutHiding(fossilFoundPopup);
                }
            }
            else
            {
                Debug.Log("No fossils found near player");
            }
        }
    }

    public void DigFossil()
    {
        StartCoroutine(DigFossilCoroutine());
        homePageController.BackOneMenu();
    }

    IEnumerator DigFossilCoroutine()
    {
        string userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();

        // Create user class
        var fossil = new Fossil2();
        fossil.iD = fossilIDFound;

        // Create JSON from class
        string json = JsonUtility.ToJson(fossil);

        Debug.Log(json);

        // Create web request
        var request = new UnityWebRequest(API_URI + "fossil/dig?userID=" + userID, "PUT"); // TODO: Deploy API and change request URI accordingly

        // Encode JSON to send in the request and change content type on request header accordingly
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");

        // Make the request and check for its success
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            Debug.Log(request.downloadHandler.text);
            request.Dispose();
        }
        else
        {
            Debug.Log("Fossil dug successfully!");
            request.Dispose();
        }
    }
}
