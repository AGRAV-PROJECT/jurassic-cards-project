using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Go to Battle page
    public void ChangeToBattlePage()
    {
        SceneManager.LoadScene(9);
    }

    //Go to Scan page
    public void ChangeToScanPage()
    {
        SceneManager.LoadScene(10);
    }
}
