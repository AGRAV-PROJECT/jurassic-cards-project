using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CardsController : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {

    }

    // Check if there is a signed in user. If so, he should be redirected to the home page
    void Start()
    {
        if (PlayerPrefs.GetInt("Current_Logged_UserID", 0) == 0)
        {
            Debug.Log("No logged in user");
        }
        else
        {
            SceneManager.LoadScene(8);
        }
    }

    // Card
    public class Card
    {
        public string cardID;
        public string image;
    }


    // Call Get Cards IEnumerator
    public void GetCards()
    {
        StartCoroutine(GetCardsCoroutine());
    }

    // Get Cards logic
    IEnumerator GetCardsCoroutine()
    {
        // Get text input fields
        /*  string email = emailGameObject.text.ToString();
         string password = passwordGameObject.text.ToString();
  */

        //Get UserID
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);

        // Create user class
        /*  var card = new Card();
         card.cardID = cardID;
         card.image = image; */

        // Call Get Cards Endpoint
        string uri = "http://127.0.0.1:5000/cards/collection/" + userID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {

            // Create JSON from class
            /* string json = JsonUtility.ToJson(user);
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            webRequest.SetRequestHeader("Content-Type", "application/json");
 */
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
                    webRequest.Dispose();
                    break;
            }
        }
    }


}
