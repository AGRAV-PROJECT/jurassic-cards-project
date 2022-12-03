using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScanCardController : MonoBehaviour
{
    //Go to Home page
    public void ChangeToHomePage()
    {
        SceneManager.LoadScene(9);
    }
}
