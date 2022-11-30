using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateProfileController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Creat New profile
    public void CreateProfile(int n)
    {
        SceneManager.LoadScene(n);
    }

    //Skip this part
    public void Skip(int n)
    {
        SceneManager.LoadScene(n);
    }
}
