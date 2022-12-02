using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomePageController : MonoBehaviour
{
    List<GameObject> menuNavigation = new List<GameObject>();

    public void OpenMenu(GameObject menu)
    {
        menu.SetActive(true);

        if (menuNavigation.Count > 0)
        {
            menuNavigation[menuNavigation.Count - 1].SetActive(false);
        }

        menuNavigation.Add(menu);
    }

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
