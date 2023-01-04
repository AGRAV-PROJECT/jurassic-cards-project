using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;


public class RegisterController : MonoBehaviour
{
    // Text fields
    public Text usernameTextField;
    public Text emailTextField;
    public Text passwordTextField;
    public Text confirmPasswordTextField;
    public InputField passwordInputField;
    public InputField confirmPasswordInputField;

    // Check if user is loggedin or not
    bool isLoggedIn = false;

    // User
    public class User
    {
        public string name;
        public string password;
        public string email;
    }

    public class Player : User
    {
        public int userID;
        public int battlesFought;
        public int battlesWon;
        public int playerRank;
    }

    // Start is called before the first frame update
    void Start()
    {
        string currentUser = PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();
        Debug.Log(currentUser);
        if (currentUser == 0.ToString())
        {
            Debug.Log("No user is currenlty logged in");
        }
        else
        {
            Debug.Log("User with UID " + currentUser.ToString() + " is currently logged in");
            isLoggedIn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Go to login page
    public void ChangeToLoginPage()
    {
        SceneManager.LoadScene(2);
    }

    //Go to Terms page
    public void ChangeToTermsPage()
    {
        SceneManager.LoadScene(3);
    }

    //Go to Create Profile page
    public void ChangeToCreateProfilePage()
    {
        //LoadConfig();
        StartCoroutine(CreateUser());
    }

    IEnumerator CreateUser()
    {
        // Get text input fields
        string userName = usernameTextField.text.ToString();
        string email = emailTextField.text.ToString();
        //string password = passwordTextField.text.ToString(); // This does not work, it returns the *s, we want the text value of the input field, not the text
        //string confirmPassword = confirmPasswordTextField.text.ToString();
        string password = passwordInputField.text;
        string confirmPassword = confirmPasswordInputField.text;

        // Create user class
        var user = new User();
        user.name = userName;
        user.email = email;
        user.password = password;

        // Check if passwords match
        if (password != confirmPassword)
        {
            Debug.Log("Passwords do not match");
        }
        else
        {
            // Create JSON from class
            string json = JsonUtility.ToJson(user);

            // Create web request
            var request = new UnityWebRequest("https://jurassic-cards.herokuapp.com/account/signup", "POST");

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
                request.Dispose();
            }
            else
            {
                Debug.Log("User registered successfully!");
                request.Dispose();
            }

            // Get userID
            string uri = "https://jurassic-cards.herokuapp.com/account/getCurrentUserID/" + userName;
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
                        //Debug.Log("Username" + pages[page] + ":\UID: " + webRequest.downloadHandler.text);

                        // Save UID as "session cookie"
                        PlayerPrefs.SetInt("Current_Logged_UserID", int.Parse(webRequest.downloadHandler.text));
                        isLoggedIn = true;
                        Debug.Log(PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString());
                        webRequest.Dispose();
                        SceneManager.LoadScene(5);
                        break;
                }
            }
        }

    }

    //Go back to landing page
    public void BackToLandingPage()
    {
        SceneManager.LoadScene(0);
    }
}
