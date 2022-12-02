using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleController : MonoBehaviour
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
