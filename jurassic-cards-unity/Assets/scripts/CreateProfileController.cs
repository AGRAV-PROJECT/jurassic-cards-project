using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;

public class CreateProfileController : MonoBehaviour
{
    // Current user id
    string currentUserName = "";

    // Save changes and go to home page
    public void SaveChanges()
    {
        // Add necessary code for saving changes
        //ChangeToHomePage();
        StartCoroutine(GetCurrentUserName());
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
        string uri = "http://127.0.0.1:5000/account/getCurrentUserName/" + PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();
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
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    currentUserName = webRequest.downloadHandler.text;
                    Debug.Log(currentUserName);
                    webRequest.Dispose();
                    break;
            }
        }
    }
}
