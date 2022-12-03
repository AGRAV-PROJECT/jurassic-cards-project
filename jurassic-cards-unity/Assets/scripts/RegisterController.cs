using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RegisterController : MonoBehaviour
{
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
        SceneManager.LoadScene(5);
    }

    //Go back to landing page
    public void BackToLandingPage()
    {
        SceneManager.LoadScene(0);
    }
}
