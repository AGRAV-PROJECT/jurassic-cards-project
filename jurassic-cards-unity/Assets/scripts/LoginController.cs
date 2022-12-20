using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{

    // User
    public class User
    {
        public string password;
        public string email;
    }

    // Check if there is a signed in user. If so, he should be redirected to the home page
    void Start()
    {
        if(PlayerPrefs.GetInt("Current_Logged_UserID", 0) == 0)
        {
            Debug.Log("No logged in user");
        }
        else
        {
            SceneManager.LoadScene(8);
        }
    }

    public Text emailGameObject, passwordGameObject;

    public void LoginFunction()
    {
        StartCoroutine(Login());
    }

    // Login logic
    IEnumerator Login()
    {
        // Get text input fields
        string email    = emailGameObject.text.ToString();
        string password = passwordGameObject.text.ToString();

        // Create user class
        var user      = new User();
        user.email    = email;
        user.password = password;

        // Login
        string uri = "http://127.0.0.1:5000/account/login";
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            // Create JSON from class
            string json = JsonUtility.ToJson(user);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
            
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
                    //Debug.Log("Username" + pages[page] + ":\UID: " + webRequest.downloadHandler.text);
                    
                    // Save UID as "session cookie"
                    PlayerPrefs.SetInt("Current_Logged_UserID", int.Parse(webRequest.downloadHandler.text));
                    Debug.Log(PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString());
                    webRequest.Dispose();
                    SceneManager.LoadScene(5);
                    break;
            }
        }
    }
    
    //Go to Home page
    public void ChangeToHomePage()
    {
        SceneManager.LoadScene(8);
    }

    //Go to Register page
    public void ChangeToRegisterPage()
    {
        SceneManager.LoadScene(4);
    }


    //Go to RecoverPass page
    public void ChangeToRecoverLoginPage()
    {
        SceneManager.LoadScene(7);
    }

    //Go back to landing page
    public void BackToLandingPage()
    {
        SceneManager.LoadScene(0);
    }
}
