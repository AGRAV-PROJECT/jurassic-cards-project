using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavegationController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Return to Landing page
    public void ReturnToLandigPage()
    {
        SceneManager.LoadScene(0);
    }

    //Return to Register page
    public void ReturnToRegisterPage()
    {
        SceneManager.LoadScene(4);
    }

}
