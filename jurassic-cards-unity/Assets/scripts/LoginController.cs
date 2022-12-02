using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
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
}
