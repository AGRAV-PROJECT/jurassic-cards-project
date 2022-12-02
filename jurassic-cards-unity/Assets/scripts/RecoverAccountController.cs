using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RecoverAccountController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Go to Recover pass page
    public void ChangeToRecoverPassPage()
    {
        SceneManager.LoadScene(6);
    }
}
