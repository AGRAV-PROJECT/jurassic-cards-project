using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QRcodes : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    private int cardIdQRcode = -1;

    // Check if user is loggedin or not
    bool isLoggedIn = false;

    public class QRcode
    {
        public int cardID;
        public string playerID;
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
        public string ownedSince;
        public string image;
        public bool hasBeenRedeemed;
    }

    void Start()
    {
        string currentUser = PlayerPrefs.GetInt("Current_Logged_UserID", 0).ToString();
        Debug.Log(currentUser);
        if (currentUser == 0.ToString())
        {
            Debug.Log("No user is currenlty logged in");
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
        Found();
    }


    //if some qr code is found
    public void Found()
    {
        //if qr code 13 is found
        if (text.text == "card13")
        {
            Debug.Log("found qr code 13");
            cardIdQRcode = 13;
            CreateJSONforCard13();
            //StartCoroutine(UploadJsonQrCode13());
        }
        //if qr code 14 is found
        if (text.text == "card14") 
        {
            cardIdQRcode = 14;
            CreateJSONforCard14();
            Debug.Log("found qr code 14");
        }

        if (text.text == "card15")
        {
            cardIdQRcode = 15;
            CreateJSONforCard15();
            Debug.Log("found qr code 15");
        }

        if (text.text == "card16")
        {
            cardIdQRcode = 16;
            CreateJSONforCard16();
            Debug.Log("found qr code 16");
        }
        //other qr codes...
        else
        {
            Debug.Log("qr code not found");
        }
    }

    public void CreateJSONforCard13()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 13;
        qrcode.playerID = "replaceWithCurrentPlayerID";
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
        qrcode.ownedSince = "replaceWithCurrentDate";
        qrcode.image = "replaceWithPlesiossaurosImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;
    }

    public void CreateJSONforCard14()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 14;
        qrcode.playerID = "replaceWithCurrentPlayerID";
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
        qrcode.ownedSince = "replaceWithCurrentDate";
        qrcode.image = "replaceWithPlesiossaurosImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;
    }

    public void CreateJSONforCard15()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 15;
        qrcode.playerID = "replaceWithCurrentPlayerID";
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
        qrcode.ownedSince = "replaceWithCurrentDate";
        qrcode.image = "replaceWithPlesiossaurosImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;
    }

    public void CreateJSONforCard16()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 16;
        qrcode.playerID = "replaceWithCurrentPlayerID";
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
        qrcode.ownedSince = "replaceWithCurrentDate";
        qrcode.image = "replaceWithPlesiossaurosImageFromUnityAssets";
        qrcode.hasBeenRedeemed = false;
    }
}
