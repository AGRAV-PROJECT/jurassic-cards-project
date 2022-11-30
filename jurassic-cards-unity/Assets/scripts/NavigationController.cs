using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationController : MonoBehaviour
{
    //Load new scene
    public void LoadNewScene(int n)
    {
        SceneManager.LoadScene(n);
    }
}
