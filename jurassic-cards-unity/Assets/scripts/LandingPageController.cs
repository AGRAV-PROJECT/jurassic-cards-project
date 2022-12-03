using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class LandingPageController : MonoBehaviour
{
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
