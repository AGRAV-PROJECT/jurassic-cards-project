using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecoverAccountController : MonoBehaviour
{
    //Go to Recover pass page
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
