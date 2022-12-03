using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreateProfileController : MonoBehaviour
{
    // Save changes and go to home page
    public void SaveChanges()
    {
        // Add necessary code for saving changes
        ChangeToHomePage();
    }

    //Go to home page (can be used to skip too)
    public void ChangeToHomePage()
    {
        SceneManager.LoadScene(8);
    }
}
