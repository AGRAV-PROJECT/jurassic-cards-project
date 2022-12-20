using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageController : MonoBehaviour
{
    List<GameObject> menuNavigation = new List<GameObject>();

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

    // Logout
    public void Logout()
    {
        if(PlayerPrefs.GetInt("Current_Logged_UserID", 0) == 0)
        {
            Debug.Log("No logged in user");
        }
        else
        {
            PlayerPrefs.SetInt("Current_Logged_UserID", 0);
            Debug.Log("User Logged out");
            SceneManager.LoadScene(0);
        }
    }

    // Opens new menu without hiding the one before it. Good for overlays
    public void OpenMenuWithoutHiding(GameObject menu)
    {
        menu.SetActive(true);
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

    // Used for the base menus, in the toolbar at the bottom
    public void OpenBaseMenu(GameObject menu)
    {
        if (menu.activeInHierarchy)
        {
            CloseAllMenus();
        }
        else
        {
            CloseAllMenus();
            OpenMenu(menu);
        }
    }

    // Used to save avatar changes
    public void SaveAvatarChange()
    {
        // Add whatever code needed before this
        BackOneMenu();
    }

    // Go to Battle page
    public void ChangeToBattlePage()
    {
        SceneManager.LoadScene(9);
    }
}
