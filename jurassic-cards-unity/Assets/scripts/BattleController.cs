using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class BattleController : MonoBehaviour
{
    List<GameObject> menuNavigation = new List<GameObject>();
    bool flashModeActivated = false;
    [SerializeField] private Text flashModeTextOn;
    [SerializeField] private Text flashModeTextOff;

    // Toggles flash mode
    public void FlashModeToggle()
    {
        if (flashModeActivated)
        {
            // Turn off
            flashModeTextOff.gameObject.SetActive(true);
            flashModeTextOn.gameObject.SetActive(false);
            flashModeActivated = false;
        }
        else
        {
            // Turn on
            flashModeTextOn.gameObject.SetActive(true);
            flashModeTextOff.gameObject.SetActive(false);
            flashModeActivated = true;
        }
    }

    // Opens new menu, hiding the one before it
    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);

        if (menuNavigation.Count > 0)
        {
            menuNavigation[menuNavigation.Count - 1].SetActive(false);
        }

        menuNavigation.Add(menu);
    }

    // Hides current menu and reveals previous one
    public void BackOneMenu()
    {
        if (menuNavigation.Count > 0)
        {
            if (menuNavigation.Count > 1)
            {
                menuNavigation[menuNavigation.Count - 2].SetActive(true);
            }

            menuNavigation[menuNavigation.Count - 1].SetActive(false);
            menuNavigation.RemoveAt(menuNavigation.Count - 1);
        }
    }

    // Hides all menus
    public void CloseAllMenus()
    {
        foreach (GameObject menu in menuNavigation)
        {
            menu.SetActive(false);
        }
        menuNavigation.Clear();
    }

    //Got to scan page
    public void GoToScanPage()
    {
        SceneManager.LoadScene(10);
    }

    //Return to Home page
    public void ReturnToHomePage()
    {
        SceneManager.LoadScene(8);
    }


}
