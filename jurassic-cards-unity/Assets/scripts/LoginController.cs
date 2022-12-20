using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{

    public class User
    {
        public string email;
        public string password;
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

    // Login logic
    public void Login()
    {
        
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
