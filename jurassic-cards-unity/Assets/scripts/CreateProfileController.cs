using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CreateProfileController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";

    // Current user id
    string currentUserName = "";

    void Start()
    {
        StartCoroutine(GetCurrentUserName());
    }

    // Save changes and go to home page
    public void SaveChanges()
    {
        // Add necessary code for selecting and saving avatar
        ChangeToHomePage();
    }
    public void SkipForNow()
    {
        ChangeToHomePage();
    }

    //Go to home page (can be used to skip too)
    public void ChangeToHomePage()
    {
        SceneManager.LoadScene(8);
    }

    // Get the current logged in player username
    IEnumerator GetCurrentUserName()
    {
        if(PlayerPrefs.GetInt("Current_Logged_UserID", 0) == 0)
        {
            Debug.Log("No logged in user");
            yield break;
        }
        // Get userName
        string uri = API_URI + "account/getCurrentUserName/" + PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();
            
            // Get the current username that is being sent on the URI
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            
            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    currentUserName = "";
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    currentUserName = "";
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    currentUserName = "";
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    currentUserName = webRequest.downloadHandler.text;
                    Debug.Log(currentUserName);
                    webRequest.Dispose();
                    break;
                default:
                    Debug.Log("Other error (user may not exist)");
                    currentUserName = "";
                    webRequest.Dispose();
                    break;
            }
        }
    }
}
