using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class QRcodes : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;

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
        //if qr code 1 is found
        if (text.text == "found1")
        {
            Debug.Log("found qr code 1");
            StartCoroutine(UploadJsonQrCode1());
        }
        //if qr code 2 is found
        else if (text.text == "found2") 
        {
            Debug.Log("found qr code 2");
            //StartCoroutine(UploadJsonQrCode2());
        }
        //other qr codes...
        else
        {
            Debug.Log("qr code not found");
        }
    }

    //upload json for the qr code 1
    IEnumerator UploadJsonQrCode1()
    {
        var qrcode = new QRcode();
        qrcode.cardID = 1;
        qrcode.playerID = "placeholder";
        qrcode.name = "Jane Doe";
        qrcode.description = "Desc";
        qrcode.combatPoints = 5;
        qrcode.level = 1;
        qrcode.agility = 5;
        qrcode.attack = 6;
        qrcode.health = 7;
        qrcode.ability1 = "Scratch";
        qrcode.ability2 = "Growl";
        qrcode.ability3 = "Kick";
        qrcode.ownedSince = "placeholder";
        qrcode.image = "placeholderImage";


        string json = JsonUtility.ToJson(qrcode);

        var req = new UnityWebRequest("http://127.0.0.1:5000/cards/scan", "POST");
        byte[] jsonToSend = new System.Text.UTF8Encoding().GetBytes(json);
        req.uploadHandler = (UploadHandler)new UploadHandlerRaw(jsonToSend);
        req.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        yield return req.SendWebRequest();
        if (req.result != UnityWebRequest.Result.Success)
        {
            Debug.Log(req.error);
            req.Dispose();
        }
        else
        {
            Debug.Log("Form upload complete!");
            req.Dispose();
        }
    }
}
