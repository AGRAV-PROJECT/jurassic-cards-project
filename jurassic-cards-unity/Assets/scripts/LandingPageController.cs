using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.Networking;

public class LandingPageController : MonoBehaviour
{
    string currentUserName = "";

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

    //Go to login page
    public void ChangeToLoginPage()
    {
        SceneManager.LoadScene(2);
    }

    //Go to Register page
    public void ChangeToRegisterPage()
    {
        SceneManager.LoadScene(4);
    }

    //Go to About Us page
    public void ChangeToAboutUsPage()
    {
        SceneManager.LoadScene(1);
    }
}
