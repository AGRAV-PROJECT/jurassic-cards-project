using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class CardsController : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";
    GameObject cardsInventoryScroller;
    public Transform content;
    public GameObject cardPrefab;
    public Sprite plesioSprite;
    public Sprite trxSprite;
    public RectTransform rtTransform;

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
            //SceneManager.LoadScene(8);
        }
    }

    void OnDisable()
    {
        while(rtTransform.childCount > 0)
        {
            DestroyImmediate(rtTransform.GetChild(0).gameObject);
        }
    }

    // Card
    public class Card
    {
        public string cardID;
        public string playerID;
        public string cardDescription;
        public string combatPoints;
        public string cardLevel;
        public string agility;
        public string attack;
        public string health;
        public string ability1;
        public string ability2;
        public string ability3;
        public string ownDate;
        public string image;

        public string toString()
        {
            return cardID + "-" + playerID + "-" + cardLevel + "-" + ownDate;
        }
    }


    // Call Get Cards IEnumerator
    public void GetCards()
    {
        StartCoroutine(GetCardsCoroutine());
    }

    // Cards list
    [Serializable]
    public class JsonCardList
    {
        public List<JsonCard> cardList;
    }

    // JSON Card class
    [Serializable]
    public class JsonCard
    {
        public int cardID;
        public string image;
    }

    // Get Cards logic
    IEnumerator GetCardsCoroutine()
    {   
        // Initiate JSON string
        string requestResult = "";

        // Get UserID
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);

        // Call Get Cards Endpoint
        string uri = "https://jurassic-cards.herokuapp.com/cards/collection/" + userID.ToString();
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

        // Take care of JSON
        JsonCardList CardsList = new JsonCardList();
        CardsList = JsonUtility.FromJson<JsonCardList>("{\"cardList\":" + requestResult.ToString() + "}");

        foreach(JsonCard item in CardsList.cardList)
        {
            if(item.image.Contains("Plesiossauros"))
            {
                GameObject obj = Instantiate(cardPrefab, content);
                var itemName = obj.transform.Find("CardText").GetComponent<Text>();
                var itemIcon = obj.transform.Find("CardImage").GetComponent<Image>();

                itemName.text = "Plesiosauro";
                itemIcon.sprite = plesioSprite;

                var objectButton = obj.GetComponent<Button>();
                //objectButton.onClick.AddListener(CheckDescription);
                objectButton.onClick.AddListener(delegate{CheckDescription(item.cardID);});
            }
            else if(item.image.Contains("Triceratops"))
            {
        
            }
            else if(item.image.Contains("TRex"))
            {
                GameObject obj = Instantiate(cardPrefab, content);
                var itemName = obj.transform.Find("CardText").GetComponent<Text>();
                var itemIcon = obj.transform.Find("CardImage").GetComponent<Image>();

                itemName.text = "T-Rex";
                itemIcon.sprite = trxSprite;
                var objectButton = obj.GetComponent<Button>();
                objectButton.onClick.AddListener(delegate{CheckDescription(item.cardID);});
            }
            else if(item.image.Contains("Spinosauros"))
            {
        
            }
        }
    }

    // OnClick card button
    public void CheckDescription(int cardID)
    {
        Debug.Log("TESTINGGGGGGGGGGGGGGGGGGGGGGG " + cardID.ToString());
    }

    // Clear scrollview
    public void DestroyChildren()
    {   

        while(rtTransform.childCount > 0)
        {
            DestroyImmediate(rtTransform.GetChild(0).gameObject);
        }

    }

    // Call Scan Card IEnumerator
    //public void ScanCard(string json)
    //{
    //    Debug.Log("JSON QR CODE: \n" + json);
    //
    //    StartCoroutine(ScanCardCoroutine(json)); // erro - Null Exception
    //}

    // Scan Card
    /* public IEnumerator ScanCardCoroutine(string json)
    {

        //Get UserID
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        Debug.Log("userID Lido Dentro Coroutine:" + userID);

        yield return 0; //------------------ APAGAR PÓS TESTE

        // Create card class
        var card = new Card();
        card.cardID = cardID;
        card.cardDescription = cardDescription;
        card.combatPoints = combatPoints;
        card.cardLevel = cardLevel;
        card.agility = agility;
        card.attack = attack;
        card.health = health;
        card.ability1 = ability1;
        card.ability2 = ability2;
        card.ability3 = ability3;
        card.ownDate = ownDate;
        card.image = image;


        
                // Call Scan Card Endpoint
                string uri = API_URL + "cards/scan/" + userID;
                using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
                {

                    // Create JSON from class
                    //string json = JsonUtility.ToJson(card);
                    byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
                    webRequest.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
                    webRequest.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
                    webRequest.SetRequestHeader("Content-Type", "application/json");

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
                            //Display success of the operation to the user
                            webRequest.Dispose();
                            break;
                    }
                } 
    }*/

    // Call Evolve Card IEnumerator
    public void EvolveCard()
    {
        StartCoroutine(EvolveCardCoroutine());
    }

    // Evolve Card
    IEnumerator EvolveCardCoroutine()
    {


        //Get CardID
        int cardID = 3; // Get the id of the card chosen by the player

        // Call Evolve Card Endpoint /PUT
        string uri = API_URI + "cards/evolve?cardID=" + cardID;
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // Get the current username that is being sent on the URI
            /* string[] pages = uri.Split('/');
            int page = pages.Length - 1;
            */

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
                    //Display success of the operation to the user
                    webRequest.Dispose();
                    break;
            }
        }
    }

    // Call Get Cards Info IEnumerator
    public void GetCardInfo()
    {
        StartCoroutine(GetCardInfoCoroutine());
    }

    // Get Card Info logic
    IEnumerator GetCardInfoCoroutine()
    {

        //Get CardID 
        // TBD
        int cardID = 3; //carta que o user escolher para visualizar


        // Call Get Cards info Endpoint
        string uri = API_URI + "cards/check/" + cardID;
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
                    Card card = JsonUtility.FromJson<Card>(webRequest.downloadHandler.text);

                    Debug.Log(card.ToString());

                    /* // Create card class
                    var card = new Card();
                    card.cardID = cardID;
                    card.image = image;
 */

                    webRequest.Dispose();
                    break;
            }
        }
    }


    // Call Get Fav Cards IEnumerator
    public void GetPlayerFavCard()
    {
        StartCoroutine(GetPlayerFavCardCoroutine());
    }

    // Get Player's Favorite Cards logic
    IEnumerator GetPlayerFavCardCoroutine()
    {

        //Get UserID
        int userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);


        // Call Get Fav Cards Endpoint
        string uri = API_URI + "cards/collection/favourite/" + userID;
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
            /* string[] pages = uri.Split('/');
            int page = pages.Length - 1;
 */
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

                    //Returns a json array
                    //TBD
                    //Card card = JsonUtility.FromJson<Card>(webRequest.downloadHandler.text);


                    /* // Create card class
                    var card = new Card();
                    card.cardID = cardID;
                    card.image = image;
 */

                    webRequest.Dispose();
                    break;
            }
        }
    }

    // Call Delete Card IEnumerator
    public void DeleteCard()
    {
        StartCoroutine(DeleteCardCoroutine());
    }

    // Delete Card IEnumerator logic
    IEnumerator DeleteCardCoroutine()
    {

        //Get CardID
        int cardID = 5; // REPLACE HERE, card that the user clickes/chooses to delete


        // Call Delete Card Endpoint
        string uri = API_URI + "cards/delete?cardID=" + cardID;
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
            /* string[] pages = uri.Split('/');
            int page = pages.Length - 1;
 */
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
                    //Display success of the operation to the user
                    // 
                    webRequest.Dispose();
                    break;
            }
        }
    }

}
