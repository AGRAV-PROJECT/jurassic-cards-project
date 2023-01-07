using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScanCardController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";
    
    //Go to Home page
    public void ChangeToHomePage()
    {
        SceneManager.LoadScene(9);
    }
}
