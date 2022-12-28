using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoogleMaps : MonoBehaviour
{
    public RawImage img;
    public Text errortext;
    private string url;
    private bool isClickedInsertFossil = false;
    public Button buttonPlantFossil, buttonPlantFossilSubmit;
    public GameObject insertFosilPanel;
    public Text fossillong;
    public Text fossillat;

    public double lon = 41.18089601323396;
    public double lat = -8.605617846739788;

    public float timeRemaining = 10;
    public bool timerIsRunning = false;
    public double zoom = 17;
    public double width = 500;
    public double height = 640;
    public double scale = 4;


    // Start is called before the first frame update
    void Start()
    {
        //img = gameObject.GetComponent<RawImage>();
        timerIsRunning = true;
        StartCoroutine(GPSUserLocation());
    }

    // Update is called once per frame
    void Update()
    {

        if (timerIsRunning)
        {
            if (timeRemaining > 0)
            {
                //if the countdown have yet to reach 0 the timer will continue to countdown

                timeRemaining -= Time.deltaTime;
            }
            else
            {
                //when the timer reaches 0 the map will be refreshed and the timer will restart
                StartCoroutine(Map());
                timeRemaining = 10;
                timerIsRunning = true;
            }
        }
        buttonPlantFossilSubmit.onClick.AddListener(delegate { InsertFossilClick(); });
        buttonPlantFossil.onClick.AddListener(delegate { OpenPanelAddFossil(); });

    }

    public void InsertFossilClick()
    {
        isClickedInsertFossil = true;
        buttonPlantFossil.gameObject.SetActive(true);
        Debug.Log("aqui");
    }
    IEnumerator Map()
    {
        //url = "https://www.google.com/maps/d/viewer?mid=15ypXVWq7IulnU49lDTVX1jWtCz68svU&ll=41.182212237519906%2C-8.608684771722723&z=17";
        //url = "https://cdn.pixabay.com/photo/2015/04/23/22/00/tree-736885__480.jpg";
        //url = "https://www.google.pt/maps/@41.1734895,-8.6076807,17z";
        //url = "https://www.google.com/maps/d/u/0/edit?mid=15ypXVWq7IulnU49lDTVX1jWtCz68svU&ll=41.18089601323396%2C-8.605617846739788&z=17";
        //url = "https://maps.googleapis.com/maps/api/staticmap?center=" + "41.18089601323396" + "," + "-8.605617846739788" +
        //"&zoom=" + "10" + "&size=" + "500" + "x" + "500" + "&scale=" + "4" + "&maptype=" + "roadmap" +
        //"&markers=color:green%7Clabel:D%7C" + "41.18089601323396" + "," + "-8.605617846739788" + "&key=AIzaSyA1-laJALAbLoiSNXQ7ZVgMS1PpJ297HJw";

        url = "https://maps.googleapis.com/maps/api/staticmap?center=" + lon + "," + lat + "&zoom=" + zoom + "&size=" + width + "x" + height + "&scale=" + scale + "&maptype=roadmap&style=element:geometry%7Ccolor:0x1d2c4d&style=element:labels.text.fill%7Ccolor:0x8ec3b9&style=element:labels.text.stroke%7Ccolor:0x1a3646&style=feature:administrative.country%7Celement:geometry.stroke%7Ccolor:0x4b6878&style=feature:administrative.land_parcel%7Celement:labels.text.fill%7Ccolor:0x64779e&style=feature:administrative.province%7Celement:geometry.stroke%7Ccolor:0x4b6878&style=feature:landscape.man_made%7Celement:geometry.stroke%7Ccolor:0x334e87&style=feature:landscape.natural%7Celement:geometry%7Ccolor:0x023e58&style=feature:poi%7Celement:geometry%7Ccolor:0x283d6a&style=feature:poi%7Celement:labels.text.fill%7Ccolor:0x6f9ba5&style=feature:poi%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:poi.park%7Celement:geometry.fill%7Ccolor:0x023e58&style=feature:poi.park%7Celement:labels.text.fill%7Ccolor:0x3C7680&style=feature:road%7Celement:geometry%7Ccolor:0x304a7d&style=feature:road%7Celement:labels.text.fill%7Ccolor:0x98a5be&style=feature:road%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:road.highway%7Celement:geometry%7Ccolor:0x2c6675&style=feature:road.highway%7Celement:geometry.stroke%7Ccolor:0x255763&style=feature:road.highway%7Celement:labels.text.fill%7Ccolor:0xb0d5ce&style=feature:road.highway%7Celement:labels.text.stroke%7Ccolor:0x023e58&style=feature:transit%7Celement:labels.text.fill%7Ccolor:0x98a5be&style=feature:transit%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:transit.line%7Celement:geometry.fill%7Ccolor:0x283d6a&style=feature:transit.station%7Celement:geometry%7Ccolor:0x3a4762&style=feature:water%7Celement:geometry%7Ccolor:0x0e1626&style=feature:water%7Celement:labels.text.fill%7Ccolor:0x4e6d70&size=480x360&key=AIzaSyA1-laJALAbLoiSNXQ7ZVgMS1PpJ297HJw&markers=color:green%7Clabel:D%7C41.18089601323396,-8.605617846739788&key=AIzaSyA1-laJALAbLoiSNXQ7ZVgMS1PpJ297HJw";


        var index = url.IndexOf("&zoom=");
        if(isClickedInsertFossil)
        {
            if (index >= 0)
            {
                url = url.Insert(index, "&markers=color:blue%7Clabel:F%7C" + fossillong.text + "," + fossillat.text + "&key=AIzaSyA1-laJALAbLoiSNXQ7ZVgMS1PpJ297HJw");
                Debug.Log("url3: " + url);
                isClickedInsertFossil = false;
                insertFosilPanel.SetActive(false);
            }
        }

        //url = "https://maps.googleapis.com/maps/api/staticmap?center=41.18089601323396,-8.605617846739788&zoom=10&size=500x500&scale=4&maptype=roadmap&style=element:geometry%7Ccolor:0x1d2c4d&style=element:labels.text.fill%7Ccolor:0x8ec3b9&style=element:labels.text.stroke%7Ccolor:0x1a3646&style=feature:administrative.country%7Celement:geometry.stroke%7Ccolor:0x4b6878&style=feature:administrative.land_parcel%7Celement:labels.text.fill%7Ccolor:0x64779e&style=feature:administrative.province%7Celement:geometry.stroke%7Ccolor:0x4b6878&style=feature:landscape.man_made%7Celement:geometry.stroke%7Ccolor:0x334e87&style=feature:landscape.natural%7Celement:geometry%7Ccolor:0x023e58&style=feature:poi%7Celement:geometry%7Ccolor:0x283d6a&style=feature:poi%7Celement:labels.text.fill%7Ccolor:0x6f9ba5&style=feature:poi%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:poi.park%7Celement:geometry.fill%7Ccolor:0x023e58&style=feature:poi.park%7Celement:labels.text.fill%7Ccolor:0x3C7680&style=feature:road%7Celement:geometry%7Ccolor:0x304a7d&style=feature:road%7Celement:labels.text.fill%7Ccolor:0x98a5be&style=feature:road%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:road.highway%7Celement:geometry%7Ccolor:0x2c6675&style=feature:road.highway%7Celement:geometry.stroke%7Ccolor:0x255763&style=feature:road.highway%7Celement:labels.text.fill%7Ccolor:0xb0d5ce&style=feature:road.highway%7Celement:labels.text.stroke%7Ccolor:0x023e58&style=feature:transit%7Celement:labels.text.fill%7Ccolor:0x98a5be&style=feature:transit%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:transit.line%7Celement:geometry.fill%7Ccolor:0x283d6a&style=feature:transit.station%7Celement:geometry%7Ccolor:0x3a4762&style=feature:water%7Celement:geometry%7Ccolor:0x0e1626&style=feature:water%7Celement:labels.text.fill%7Ccolor:0x4e6d70&size=480x360&key=AIzaSyA1-laJALAbLoiSNXQ7ZVgMS1PpJ297HJw";

        //url = "https://maps.googleapis.com/maps/api/staticmap?key=AIzaSyA1-laJALAbLoiSNXQ7ZVgMS1PpJ297HJw&center=41.17003932872469,-8.607315897900389&zoom=13&format=png&maptype=roadmap&style=element:geometry%7Ccolor:0x1d2c4d&style=element:labels.text.fill%7Ccolor:0x8ec3b9&style=element:labels.text.stroke%7Ccolor:0x1a3646&style=feature:administrative.country%7Celement:geometry.stroke%7Ccolor:0x4b6878&style=feature:administrative.land_parcel%7Celement:labels.text.fill%7Ccolor:0x64779e&style=feature:administrative.province%7Celement:geometry.stroke%7Ccolor:0x4b6878&style=feature:landscape.man_made%7Celement:geometry.stroke%7Ccolor:0x334e87&style=feature:landscape.natural%7Celement:geometry%7Ccolor:0x023e58&style=feature:poi%7Celement:geometry%7Ccolor:0x283d6a&style=feature:poi%7Celement:labels.text.fill%7Ccolor:0x6f9ba5&style=feature:poi%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:poi.park%7Celement:geometry.fill%7Ccolor:0x023e58&style=feature:poi.park%7Celement:labels.text.fill%7Ccolor:0x3C7680&style=feature:road%7Celement:geometry%7Ccolor:0x304a7d&style=feature:road%7Celement:labels.text.fill%7Ccolor:0x98a5be&style=feature:road%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:road.highway%7Celement:geometry%7Ccolor:0x2c6675&style=feature:road.highway%7Celement:geometry.stroke%7Ccolor:0x255763&style=feature:road.highway%7Celement:labels.text.fill%7Ccolor:0xb0d5ce&style=feature:road.highway%7Celement:labels.text.stroke%7Ccolor:0x023e58&style=feature:transit%7Celement:labels.text.fill%7Ccolor:0x98a5be&style=feature:transit%7Celement:labels.text.stroke%7Ccolor:0x1d2c4d&style=feature:transit.line%7Celement:geometry.fill%7Ccolor:0x283d6a&style=feature:transit.station%7Celement:geometry%7Ccolor:0x3a4762&style=feature:water%7Celement:geometry%7Ccolor:0x0e1626&style=feature:water%7Celement:labels.text.fill%7Ccolor:0x4e6d70&size=480x360";

        WWW www = new WWW(url);

        yield return www;
        img.texture = www.texture;
        img.SetNativeSize();
    }

    public void OpenPanelAddFossil()
    {
        insertFosilPanel.SetActive(true);
        buttonPlantFossil.gameObject.SetActive(false);
    }
    IEnumerator GPSUserLocation()
    {
        if(!Input.location.isEnabledByUser)
        {
            Debug.Log("location is not Enabled By User");
            errortext.text = "location is not Enabled By User";

            //start map with default coordinates
            Debug.Log("start map with default coordinates");
            StartCoroutine(Map());
            yield break;
        }

        //start service
        Input.location.Start();
        errortext.text = "loading map...please wait";
        //wait until service initialize
        int maxWait = 20;
        while(Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        //service didnt init at 20s
        if(maxWait < 1)
        {
            Debug.Log("erro maxWaint < 1");
            errortext.text = "erro maxWaint < 1";
            yield break;
        }

        //conection failed
        if(Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.Log("unnable to determine device location");
            errortext.text = "unnable to determine device location";

            //start map with default coordinates
            Debug.Log("start map with default coordinates");
            StartCoroutine(Map());
            yield break;
        }
        else
        {
            Debug.Log("running");
            InvokeRepeating("UpdateGPSData", 0.5f, 1f);
            //access granted
        }
    }

    private void UpdateGPSData()
    {
        if(Input.location.status == LocationServiceStatus.Running)
        {
            Debug.Log("running");
            lon = Input.location.lastData.longitude;
            lat = Input.location.lastData.latitude;

            errortext.text = "lon: " + lon + ", lat: " + lat;

            //start map with the user coordinates
            Debug.Log("start map with user coordinates");
            StartCoroutine(Map());
        }

        else
        {
            Debug.Log("stopped");
        }
    }
}
