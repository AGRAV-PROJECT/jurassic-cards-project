using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class HomePageController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";

    List<GameObject> menuNavigation = new List<GameObject>();
    public InputField deleteAccountPasswordTextField;
    public GameObject scanCardMenu;
    public GameObject settingsMenu;
    public GameObject seniorModeMenu;
    public GameObject colorblindModeMenu;
    public GameObject fossilInfoPanel;
    public Text playerName;
    public Text ranking;
    public Text totalCards;

    public bool CheckIfMenuOpen()
    {
        return menuNavigation.Count > 0;
    }

    // User
    public class Fossil
    {
        public string name;
        public string longitude;
        public string latitude;
        public string date;
        public string image;
        public string description;
        public string isPlanted;
    }

    // Update member info
    public void UpdateMemberInfo()
    {
        StartCoroutine(AddMemberInfo());
    }

    // Coroutine for member info
    IEnumerator AddMemberInfo()
    {   
        string requestResult = "";

        // Get UserID
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        
        // Web Request for ranking
        string uri = API_URI + "account/getCurrentUserRanking/" + userID.ToString();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Get the current username that is being sent on the URI
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    requestResult = webRequest.downloadHandler.text;
                    webRequest.Dispose();
                    break;
            }
        }
        // Update with ranking
        ranking.text = "Pontuação: " + requestResult.ToString();
        
        // Web Request for username
        uri = API_URI + "account/getCurrentUserName/" + userID.ToString();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Get the current username that is being sent on the URI
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    requestResult = webRequest.downloadHandler.text;
                    webRequest.Dispose();
                    break;
            }
        }
        // Update with ranking
		string requestResultFinal = "";
		for(int i=1; i<requestResult.Length - 1; i++)
		{
			requestResultFinal += requestResult[i];
		}
        playerName.text = requestResultFinal.ToString();

        // Web Request for card count
        uri = API_URI + "account/getCardCount/" + userID.ToString();
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Get the current username that is being sent on the URI
            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            // Check request result
            switch (webRequest.result)
            {
                case UnityWebRequest.Result.ConnectionError:
                    Debug.Log("ERROR");
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.DataProcessingError:
                    //Debug.LogError(pages[page] + ": Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.ProtocolError:
                    //Debug.LogError(pages[page] + ": HTTP Error: " + webRequest.error);
                    webRequest.Dispose();
                    break;
                case UnityWebRequest.Result.Success:
                    Debug.Log(webRequest.downloadHandler.text);
                    requestResult = webRequest.downloadHandler.text;
                    webRequest.Dispose();
                    break;
            }
        }
        // Update with card count
        totalCards.text = "Total Cards: " + requestResult.ToString();
    }

    // Win battle
    public void WinBattle()
    {
        StartCoroutine(BattleWon());
    }

    IEnumerator BattleWon()
    {
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        // Create web request
        var request = new UnityWebRequest(API_URI + "/account/winBattle/" + userID, "POST");

        // Make the request and check for its success
        yield return request.SendWebRequest();
        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(request.error);
            request.Dispose();
        }
        else
        {
            Debug.Log("Battle won!");
            StartCoroutine(AddMemberInfo());
            request.Dispose();
        }
    }

    // Go to Create Profile page
    public void AddFossil()
    {
        StartCoroutine(AddFossilCoroutine());
    }

    IEnumerator AddFossilCoroutine()
    {
        string userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();

        // Create user class
        var fossil = new Fossil();
        fossil.name = "T-Rex2";
        fossil.longitude = "0";
        fossil.latitude = "0";
        fossil.date = "0";
        fossil.image = "T-Rex";
        fossil.description = "Test";
        fossil.isPlanted = "0";

        // Create JSON from class
        string json = JsonUtility.ToJson(fossil);

        Debug.Log(json);

        // Create web request
        var request = new UnityWebRequest(API_URI + "fossil/add?userID=" + userID, "POST"); // TODO: Deploy API and change request URI accordingly

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
            Debug.Log(request.downloadHandler.text);
            request.Dispose();
        }
        else
        {
            Debug.Log("Fossil added successfully!");
            request.Dispose();
        }
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("justChangedSeniorMode", 0) == 1)
        {
            PlayerPrefs.SetInt("justChangedSeniorMode", 0);
            OpenBaseMenu(settingsMenu);
            OpenMenuWithoutHiding(seniorModeMenu);
        }
        if (PlayerPrefs.GetInt("justChangedColorblindMode", 0) == 1)
        {
            PlayerPrefs.SetInt("justChangedColorblindMode", 0);
            OpenBaseMenu(settingsMenu);
            OpenMenuWithoutHiding(colorblindModeMenu);
        }
    }

    public void ChangeLanguage(int id)
    {
        PlayerPrefs.SetInt("languageID", id);
        StartCoroutine(ChangeLanguageCoroutine(id));
    }

    IEnumerator ChangeLanguageCoroutine(int id)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[id];
    }

    public void SetSeniorMode(int value)
    {
        if (PlayerPrefs.GetInt("seniorMode", 0) == 0 && value == 1)
        {
            PlayerPrefs.SetInt("seniorMode", 1);
        } else if (PlayerPrefs.GetInt("seniorMode", 0) == 1 && value == 0)
        {
            PlayerPrefs.SetInt("seniorMode", 0);
        }
        PlayerPrefs.SetInt("justChangedSeniorMode", 1);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void SetColorblindMode(int value)
    {
        if (PlayerPrefs.GetInt("colorblindMode", 0) == 0 && value == 1)
        {
            PlayerPrefs.SetInt("colorblindMode", 1);
        }
        else if (PlayerPrefs.GetInt("colorblindMode", 0) == 1 && value == 0)
        {
            PlayerPrefs.SetInt("colorblindMode", 0);
        }
        PlayerPrefs.SetInt("justChangedColorblindMode", 1);
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void OpenScanCardMenu()
    {
        gameObject.SetActive(false);
        scanCardMenu.SetActive(true);
    }

    public void CloseScanCardMenu()
    {
        scanCardMenu.SetActive(false);
        gameObject.SetActive(true);
    }

    // Open fossil info
    public void OpenFossilInfo()
    {
        fossilInfoPanel.SetActive(true);
        menuNavigation.Add(fossilInfoPanel);
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
            var request = new UnityWebRequest(API_URI + "account/delete", "DELETE"); // TODO: Deploy API and change request URI accordingly

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
