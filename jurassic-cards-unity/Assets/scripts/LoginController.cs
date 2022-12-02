using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
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
