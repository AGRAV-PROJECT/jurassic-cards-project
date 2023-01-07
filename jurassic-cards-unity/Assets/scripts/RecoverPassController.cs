using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RecoverPassController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";
    
    // Start is called before the first frame update

    public class User
    {
        public string password;
    }

    public Text newPasswordGameObject, confirmNewPasswordGameObject;

    void Start()
    {
        
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
}
