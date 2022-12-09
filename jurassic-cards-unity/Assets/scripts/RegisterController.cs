using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.Networking;
//using System.Data.Sql;
//using System.Data.SqlClient;

using Newtonsoft.Json;

public class RegisterController : MonoBehaviour
{
    // User
    public class User
    {
        public string name;
        public string password;
        public string email;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Go to login page
    public void ChangeToLoginPage()
    {
        SceneManager.LoadScene(2);
    }

    //Go to Terms page
    public void ChangeToTermsPage()
    {
        SceneManager.LoadScene(3);
    }

    //Go to Create Profile page
    public void ChangeToCreateProfilePage()
    {
        //SceneManager.LoadScene(5);
        //LoadConfig();
        StartCoroutine(UploadRegister());
    }

    IEnumerator UploadRegister()
    {
        var user = new User();
        user.name = "vitor9";
        user.email = "vitordiogo9@sapo.pt";
        user.password = "Testing123!";

        string json = JsonUtility.ToJson(user);

        var req = new UnityWebRequest("http://127.0.0.1:5000/account/signup", "POST");
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
        
        //WWWForm form = new WWWForm();
        //form.AddField("name", "vitor3");
        //form.AddField("email", "vitordiogo20002@sapo.pt");
        //form.AddField("password", "Testing123!");
        //using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1:5000/account/signup", form))
        //{
        //    www.SetRequestHeader("Content-Type", "application/json");
        //    yield return www.SendWebRequest();
        //
        //    if (www.result != UnityWebRequest.Result.Success)
        //    {
        //        Debug.Log(www.error);
        //    }
        //    else
        //    {
        //        Debug.Log("Form upload complete!");
        //    }
        //}
    }

    //Go back to landing page
    public void BackToLandingPage()
    {
        SceneManager.LoadScene(0);
    }
}
