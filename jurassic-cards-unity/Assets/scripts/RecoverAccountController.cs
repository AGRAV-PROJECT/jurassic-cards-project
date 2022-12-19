using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecoverAccountController : MonoBehaviour
{
    //Go to Recover pass page

    public class User
    {
        public string email;
    }

    public Text emailGameObject;

    public void ChangeToRecoverPassPage()
    {
        SceneManager.LoadScene(6);
    }

    //Go back to login page
    public void BackToLoginPage()
    {
        SceneManager.LoadScene(2);
    }
}
