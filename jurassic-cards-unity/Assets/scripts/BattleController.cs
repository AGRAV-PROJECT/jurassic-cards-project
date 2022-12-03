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
    const string TEXT_FLASH_MODE_BUTTON_OFF = "FLASH MODE: OFF";
    const string TEXT_FLASH_MODE_BUTTON_ON = "FLASH MODE: ON";

    // Toggles flash mode
    public void FlashModeToggle()
    {
        if (flashModeActivated)
        {
            // Turn off
            EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text = TEXT_FLASH_MODE_BUTTON_OFF;
            flashModeActivated = false;
        }
        else
        {
            // Turn on
            EventSystem.current.currentSelectedGameObject.GetComponentInChildren<Text>().text = TEXT_FLASH_MODE_BUTTON_ON;
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
