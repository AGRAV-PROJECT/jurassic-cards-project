using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePageController : MonoBehaviour
{
    List<GameObject> menuNavigation = new List<GameObject>();
    public InputField deleteAccountPasswordTextField;

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

    //Go to Create Profile page
    public void DeleteAccount()
    {
        //LoadConfig();
        StartCoroutine(DeleteAccountCoroutine());
    }

    // User
    public class User
    {
        public string userid;
        public string password;
    }

    IEnumerator DeleteAccountCoroutine()
    {
        if (PlayerPrefs.GetInt("Current_Logged_UserID", 0) == 0)
        {
            Debug.Log("No logged in user");
        }
        else
        {
        // Get text input fields
        string password = deleteAccountPasswordTextField.text.ToString();

        // Create user class
        var user = new User();
        user.userid = PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();
        user.password = password;

            // Create JSON from class
            string json = JsonUtility.ToJson(user);

            // Create web request
            var request = new UnityWebRequest("http://127.0.0.1:5000/account/delete", "DELETE"); // TODO: Deploy API and change request URI accordingly

            // Encode JSON to send in the request and change content type on request header accordingly
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Make the request and check for its success
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(request.error);
                request.Dispose();
            }
            else
            {
                Debug.Log("User deleted successfully!");
                request.Dispose();
                Logout();
            }
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
