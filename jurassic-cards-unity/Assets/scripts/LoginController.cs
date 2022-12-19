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

    public Text emailGameObject, passwordGameObject;

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
