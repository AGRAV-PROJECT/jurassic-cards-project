using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System;

public class QRcodes : MonoBehaviour
{
    string API_URI = "https://jurassic-cards.herokuapp.com/";

    // Start is called before the first frame update
    public Text text;
    private int cardIdQRcode = -1;

    // Check if user is loggedin or not
    bool isLoggedIn = false;

    //Check if a card was scanned
    bool wasScanned = false;

    //CardsController cardsController = (new GameObject("SomeObjName")).AddComponent<CardsController>();
    
    //Popup
    public GameObject popUpMenu;

    public class QRcode
    {
        public int cardID;
        public int userID;
        public string name;
        public string description;
        public int combatPoints;
        public int level;
        public int agility;
        public int attack;
        public int health;
        public string ability1;
        public string ability2;
        public string ability3;
        public DateTime ownedSince;
        public string image;
        public bool hasBeenRedeemed;
    }

    public class Card
    {
        public int cardID;
        public string cardName;
        public string cardDescription;
        public int combatPoints;
        public int cardLevel;
        public int agility;
        public int attack;
        public int health;
        public string ability1;
        public string ability2;
        public string ability3;
        public string ownedSince;
        public string image;
    }

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

    void Start()
    {

        string currentUser = PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();
        Debug.Log(currentUser);
        if (currentUser == 0.ToString())
        {
            Debug.Log("No user is currently logged in");
        }
        else
        {
            Debug.Log("User with UID " + currentUser.ToString() + " is currently logged in");
            isLoggedIn = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!wasScanned)
        {
            Found();
        }
    }

    // Logic for adding card to DB
    IEnumerator AddCard(int tempCardID, bool isFossil)
    {
        var card = new Card();
        var fossil = new Fossil();
        if(tempCardID == 13)
        {
            card.cardID = tempCardID;
            card.cardName = "Plesiossauros";
            card.cardDescription = "PlesiossaurosDescription";
            card.combatPoints = 10;
            card.cardLevel = 1;
            card.agility = 10;
            card.attack = 6;
            card.health = 7;
            card.ability1 = "Splash";
            card.ability2 = "Bite";
            card.ability3 = "Tsunami";
            card.ownedSince = "test";
            card.image = "replaceWithPlesiossaurosImageFromUnityAssets";
        }
        if(tempCardID == 14)
        {
            card.cardID = 14;
            card.cardName = "Spinosauros";
            card.cardDescription = "SpinosaurosDescription";
            card.combatPoints = 13;
            card.cardLevel = 1;
            card.agility = 5;
            card.attack = 12;
            card.health = 7;
            card.ability1 = "Splash";
            card.ability2 = "Scratch";
            card.ability3 = "Fear";
            card.ownedSince = "test";
            card.image = "replaceWithSpinosaurosImageFromUnityAssets";
        }
        if(tempCardID == 15)
        {
            card.cardID = 15;
            card.cardName = "T-Rex";
            card.cardDescription = "TRexDescription";
            card.combatPoints = 15;
            card.cardLevel = 1;
            card.agility = 2;
            card.attack = 15;
            card.health = 6;
            card.ability1 = "Ferocious Bite";
            card.ability2 = "Stomp";
            card.ability3 = "Fear";
            card.ownedSince = "test";
            card.image = "replaceWithTRexImageFromUnityAssets";
        }
        if(tempCardID == 16)
        {
            //card.cardID = 16;
            //card.cardName = "Triceratops";
            //card.cardDescription = "TriceratopsDescription";
            //card.combatPoints = 9;
            //card.cardLevel = 1;
            //card.agility = 5;
            //card.attack = 7;
            //card.health = 13;
            //card.ability1 = "Shield Bash";
            //card.ability2 = "Protection";
            //card.ability3 = "Charge";
            //card.ownedSince = "test";
            //card.image = "replaceWithTriceratopsImageFromUnityAssets";
            Debug.Log("Nothing to see here...");

        }
        if (tempCardID == 17)
        {
            card.cardID = 17;
            card.cardName = "Plesiossauros";
            card.cardDescription = "PlesiossaurosDescription";
            card.combatPoints = 10;
            card.cardLevel = 1;
            card.agility = 10;
            card.attack = 6;
            card.health = 7;
            card.ability1 = "Splash";
            card.ability2 = "Bite";
            card.ability3 = "Tsunami";
            card.ownedSince = "test";
            card.image = "replaceWithPlesiossaurosImageFromUnityAssets";
        }
        if (tempCardID == 18)
        {
            card.cardID = 18;
            card.cardName = "Spinosauros";
            card.cardDescription = "SpinosaurosDescription";
            card.combatPoints = 13;
            card.cardLevel = 1;
            card.agility = 5;
            card.attack = 12;
            card.health = 7;
            card.ability1 = "Splash";
            card.ability2 = "Scratch";
            card.ability3 = "Fear";
            card.ownedSince = "test";
            card.image = "replaceWithSpinosaurosImageFromUnityAssets";
        }

        if(isFossil == true)
        {
            string userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();

            // Create user class
            fossil.name = "Triceratops";
            fossil.longitude = "0";
            fossil.latitude = "0";
            fossil.date = "2022-01-13";
            fossil.image = "Triceratops";
            fossil.description = "Description";
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
        else
        {
            string uri = API_URI + "cards/scan/" + PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();
            
            // Create JSON from class
            string json = JsonUtility.ToJson(card);
            // Create web request
            var request = new UnityWebRequest(uri, "POST");

            // Encode JSON to send in the request and change content type on request header accordingly
            byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
            request.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
            request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
            request.SetRequestHeader("Content-Type", "application/json");

            // Make the request and check for its success
            yield return request.SendWebRequest();
            if (request.result != UnityWebRequest.Result.Success)
            {
                if(request.downloadHandler.text.Contains("repeated"))
                {
                    Debug.Log("Card has already been redeemed");
                }
                Debug.Log(request.error);
                request.Dispose();
            }
            else
            {
                popUpMenu.SetActive(true);
                Debug.Log("Card registered successfully!");
                request.Dispose();
            }
        }
        
    }

    // Close Popup
    public void ClosePopUp()
    {
        popUpMenu.SetActive(false);
    }

    //if some qr code is found
    public void Found()
    {
        //if qr code 13 is found
        if (text.text == "card13")
        {
            StartCoroutine(AddCard(13, false));
            wasScanned = true;
            Debug.Log("found qr code 13");
            cardIdQRcode = 13;
            CreateJSONforCard13();
            //StartCoroutine(UploadJsonQrCode13());
        }

        //if qr code 14 is found
        if (text.text == "card14")
        {
            StartCoroutine(AddCard(14, false));
            wasScanned = true;
            cardIdQRcode = 14;
            CreateJSONforCard14();
            Debug.Log("found qr code 14");
        }

        if (text.text == "card15")
        {
            StartCoroutine(AddCard(15, false));
            wasScanned = true;
            cardIdQRcode = 15;
            CreateJSONforCard15();
            Debug.Log("found qr code 15");
        }

        if (text.text == "card16")
        {
            StartCoroutine(AddCard(16, true));
            wasScanned = true;
            cardIdQRcode = 16;
            CreateJSONforCard16();
            Debug.Log("found qr code 16");
        }

        if (text.text == "card17")
        {
            StartCoroutine(AddCard(17, false));
            wasScanned = true;
            cardIdQRcode = 17;
            CreateJSONforCard17();
            Debug.Log("found qr code 17");
        }

        if (text.text == "card18")
        {
            StartCoroutine(AddCard(18, false));
            wasScanned = true;
            cardIdQRcode = 18;
            CreateJSONforCard18();
            Debug.Log("found qr code 18");
        }

        //other qr codes...
        else
        {
            //Debug.Log("qr code not found");
        }
    }
    public void CallScanMethod(QRcode qrcode)
    {

        string json_qrcode = JsonUtility.ToJson(qrcode);
        //        Debug.Log("JSON QR CODE: \n" + json_qrcode);
        Debug.Log("Call scan method");
        //Call scan method in Card Controller 

        // cardController.ScanCard(json_qrcode);       ****************************************************************
    }

    public void CreateJSONforCard13()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 13;
        qrcode.userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        qrcode.name = "Plesiossauros";
        qrcode.description = "PlesiossaurosDescription";
        qrcode.combatPoints = 10;
        qrcode.level = 1;
        qrcode.agility = 10;
        qrcode.attack = 6;
        qrcode.health = 7;
        qrcode.ability1 = "Splash";
        qrcode.ability2 = "Bite";
        qrcode.ability3 = "Tsunami";
        qrcode.ownedSince = DateTime.Today;
        qrcode.image = "replaceWithPlesiossaurosImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;

        CallScanMethod(qrcode);
    }

    public void CreateJSONforCard14()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 14;
        qrcode.userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        qrcode.name = "Spinosauros";
        qrcode.description = "SpinosaurosDescription";
        qrcode.combatPoints = 13;
        qrcode.level = 1;
        qrcode.agility = 5;
        qrcode.attack = 12;
        qrcode.health = 7;
        qrcode.ability1 = "Splash";
        qrcode.ability2 = "Scratch";
        qrcode.ability3 = "Fear";
        qrcode.ownedSince = DateTime.Today;
        qrcode.image = "replaceWithSpinosaurosImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;

        CallScanMethod(qrcode);
    }

    public void CreateJSONforCard15()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 15;
        qrcode.userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        qrcode.name = "T-Rex";
        qrcode.description = "TRexDescription";
        qrcode.combatPoints = 15;
        qrcode.level = 1;
        qrcode.agility = 2;
        qrcode.attack = 15;
        qrcode.health = 6;
        qrcode.ability1 = "Ferocious Bite";
        qrcode.ability2 = "Stomp";
        qrcode.ability3 = "Fear";
        qrcode.ownedSince = DateTime.Today;
        qrcode.image = "replaceWithTRexImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;

        CallScanMethod(qrcode);
    }

    public void CreateJSONforCard16()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 16;
        qrcode.userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        qrcode.name = "Triceratops";
        qrcode.description = "TriceratopsDescription";
        qrcode.combatPoints = 9;
        qrcode.level = 1;
        qrcode.agility = 5;
        qrcode.attack = 7;
        qrcode.health = 13;
        qrcode.ability1 = "Shield Bash";
        qrcode.ability2 = "Protection";
        qrcode.ability3 = "Charge";
        qrcode.ownedSince = DateTime.Today;
        qrcode.image = "replaceWithTriceratopsImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;

        CallScanMethod(qrcode);
    }

    public void CreateJSONforCard17()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 17;
        qrcode.userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        qrcode.name = "Plesiossauros";
        qrcode.description = "PlesiossaurosDescription";
        qrcode.combatPoints = 10;
        qrcode.level = 1;
        qrcode.agility = 10;
        qrcode.attack = 6;
        qrcode.health = 7;
        qrcode.ability1 = "Splash";
        qrcode.ability2 = "Bite";
        qrcode.ability3 = "Tsunami";
        qrcode.ownedSince = DateTime.Today;
        qrcode.image = "replaceWithPlesiossaurosImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;

        CallScanMethod(qrcode);
    }

    public void CreateJSONforCard18()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 18;
        qrcode.userID = PlayerPrefs.GetInt("Current_Logged_UserID", 0);
        qrcode.name = "Spinosauros";
        qrcode.description = "SpinosaurosDescription";
        qrcode.combatPoints = 13;
        qrcode.level = 1;
        qrcode.agility = 5;
        qrcode.attack = 12;
        qrcode.health = 7;
        qrcode.ability1 = "Splash";
        qrcode.ability2 = "Scratch";
        qrcode.ability3 = "Fear";
        qrcode.ownedSince = DateTime.Today;
        qrcode.image = "replaceWithSpinosaurosImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;

        CallScanMethod(qrcode);
    }
}
