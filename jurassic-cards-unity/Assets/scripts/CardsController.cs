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
    public Sprite spinoSprite;
    public RectTransform rtTransform;
    public GameObject cardDetailMenu;
    public GameObject cardListMenu;

    // Texts for card details
    public Text cardNameText;
    public Text cardRankingText;
    public Text cardLevelText;
    public Text ability1Text;
    public Text ability2Text;
    public Text ability3Text;
    public Text attackText;
    public Text agilityText;
    public Text healthText;
    public Text descriptionText;
    public Image cardImage;

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
        try
        {
            while(rtTransform.childCount > 0)
            {
                DestroyImmediate(rtTransform.GetChild(0).gameObject);
            }
        }
        catch
        {
            Debug.Log("Already clean");
        }
        
    }

    // Card
    [Serializable]
    public class Card
    {
        public string cardID;
        public string playerID;
        public string name;
        public string description;
        public string combatPoints;
        public string level;
        public string agility;
        public string attack;
        public string health;
        public string ability1;
        public string ability2;
        public string ability3;
        public string ownedSince;
        public string image;

        public string toString()
        {
            return cardID + "-" + playerID + "-" + level + "-" + ownedSince;
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
    
    // Card List for Checking individual
    [Serializable]
    public class JsonCardListChecking
    {
        public List<Card> cardList;
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
        string uri = API_URI + "cards/collection/" + userID.ToString();
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
                // Only available on DLC
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
                // Only available on DLC
                //GameObject obj = Instantiate(cardPrefab, content);
                //var itemName = obj.transform.Find("CardText").GetComponent<Text>();
                //var itemIcon = obj.transform.Find("CardImage").GetComponent<Image>();
                //
                //itemName.text = "Spinosauros";
                //itemIcon.sprite = spinoSprite;
                //var objectButton = obj.GetComponent<Button>();
                //objectButton.onClick.AddListener(delegate{CheckDescription(item.cardID);});
            }
        }
    }

    // OnClick card button
    public void CheckDescription(int cardID)
    {
        StartCoroutine(CheckCardDescription(cardID));
    }

    IEnumerator CheckCardDescription(int cardID)
    {
        // Initiate JSON string
        string requestResult = "";

        // Call Get Cards Endpoint
        string uri = API_URI + "cards/check/" + cardID.ToString();
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
                    requestResult = webRequest.downloadHandler.text;
                    webRequest.Dispose();
                    break;
            }
        }

        // Take care of JSON
        JsonCardListChecking cardList = new JsonCardListChecking();
        Debug.Log(requestResult.ToString());
        cardList = JsonUtility.FromJson<JsonCardListChecking>("{\"cardList\":" + requestResult.ToString() + "}");

        foreach(Card cardItem in cardList.cardList)
        {
            cardDetailMenu.SetActive(true);
            cardListMenu.SetActive(false);
            
            cardNameText.text    = cardItem.name;
            cardRankingText.text = "RANKING " + cardItem.combatPoints.ToString();
            cardLevelText.text   = "Lv. " + cardItem.level.ToString();
            ability1Text.text    = cardItem.ability1;
            ability2Text.text    = cardItem.ability2;
            ability3Text.text    = cardItem.ability3;
            attackText.text      = cardItem.attack.ToString();
            agilityText.text     = cardItem.agility.ToString();
            healthText.text      = cardItem.health.ToString();

            if(cardItem.name == "Plesiossauros")
            {
                descriptionText.text = "The Sauropterygia includes a group of ancient marine reptiles known as the Plesiosauria, also known as plesiosaurs. About 203 million years ago, during the most recent Triassic Period, presumably in the Rhaetian stage, plesiosaurs first emerged. They flourished until the Cretaceous-Paleogene extinction disaster at the end of the Cretaceous Period, roughly 66 million years ago, when they became particularly widespread during the Jurassic Period. They were distributed throughout the oceans, and several species lived, at least in part, in freshwater settings. One of the first fossilized reptiles found was a plesiosaur. Scientists became aware of their unique physical characteristics at the beginning of the nineteenth century, and in 1835 they were designated as a different order. In 1821, the name Plesiosaurus was given to the first genus of these dinosaurs. More than a hundred genuine species have since been identified. Early in the twenty-first century, there have been many discoveries, which have expanded our knowledge of their physiology, social structure, and manner of existence. Plesiosaurs had a short tail and a wide, flat body. Four long flippers that were powered by powerful muscles were the result of the evolution of their limbs, which were large bony plates made of the shoulder girdle and the pelvis. Through the water, the flippers moved like wings. Plesiosaurs had living young, breathed air, and there is evidence that they had a warm-blooded metabolism. There were two primary morphological categories in plesiosaurs. Some species, known as \"plesiosauromorphs,\" were relatively slow and grabbed little sea creatures because of their long (and occasionally extremely long) necks and small heads. Others had the \"pliosauromorph\" build, with a short neck and a huge head; they were apex predators, swift hunters of large prey, some of which could grow to a length of up to seventeen meters. The two forms are connected to the Plesiosauria's stringent conventional separation into the long-necked Plesiosauroidea and the short-necked Pliosauroidea suborders. However, current research suggests that some \"long-necked\" tribes may have had some short-necked individuals, or vice versa. As a result, the labels \"pliosauromorph\" and \"plesiosauromorph,\" which are merely descriptive, have been introduced. Today, the terms \"Plesiosauroidea\" and \"Pliosauroidea\" have a more constrained meaning. Although the term \"plesiosaur\" is technically used to describe the entire phylum Plesiosauria, it is occasionally used colloquially to refer to solely the long-necked Plesiosauroidea.";
                cardImage.sprite     = plesioSprite;
            }
            if(cardItem.name == "T-Rex")
            {
                descriptionText.text = "A big theropod dinosaur genus is called Tyrannosaurus. One of the most well-known theropod species is Tyrannosaurus rex, sometimes known as T. rex or T-Rex. On the ancient island continent of Laramidia, which is now western North America, Tyrannosaurus lived. Other tyrannosaurids' ranges were substantially less than that of Tyrannosaurus. Numerous rock formations from the Maastrichtian age of the Upper Cretaceous period, which lasted from 68 to 66 million years ago, contain fossils. It was one of the final non-avian dinosaurs to exist before the Cretaceous-Paleogene extinction disaster and the last tyrannosaurid known to exist. Tyrannosaurus was a bipedal carnivore like other tyrannosaurids, with a large cranium supported by a long, hefty tail. The forelimbs of Tyrannosaurus were small but unusually powerful for their size, and they featured two clawed digits in comparison to its enormous and powerful hind limbs. Most contemporary estimates suggest that T. rex could reach lengths of over 12.4 m (40.7 ft), heights of up to 3.66-3.96 m (12-13 ft) at the hips, and a body mass of 8.87 metric tons (9.78 short tons). The most complete specimen measures up to 12.3-12.4 m (40.4-40.7 ft) in length. Tyrannosaurus rex was among the largest known land predators and is thought to have had the strongest biting force of any terrestrial animal, even though other theropods rivaled or outgrew it in size. Tyrannosaurus rex, the dominant carnivore in its ecosystem, was probably an apex predator, feeding on young armored herbivores like ceratopsians and ankylosaurs as well as maybe sauropods. According to some researchers, the dinosaur was mostly a scavenger. One of the oldest arguments in paleontology is whether Tyrannosaurus was an apex predator or only a scavenger. Today's majority of paleontologists concur that Tyrannosaurus was a skilled hunter as well as a scavenger. Some Tyrannosaurus rex specimens have almost complete skeletons. At least one of these specimens has been found to contain soft tissue and proteins. The amount of fossilized material has made it possible to conduct extensive research on a variety of biological topics, including life history and biomechanics. There is controversy on the physiology, dietary preferences, and probable speed of Tyrannosaurus rex. Its taxonomy is also debatable because some experts believe the Asian Tarbosaurus bataar is a different species of Tyrannosaurus, while others insist Tarbosaurus is a distinct genus. Tyrannosaurus has also been used as a synonym for several additional North American tyrannosaurid taxa. Since the early 20th century, Tyrannosaurus has been one of the most well-known dinosaurs as the iconic theropod and has appeared in several media, including movies, advertisements, postage stamps, and other forms of print.";
                cardImage.sprite     = trxSprite;
            }
            if(cardItem.name == "Spinosauros")
            {
                descriptionText.text = "A genus of spinosaurid dinosaurs called Spinosaurus lived in what is now North Africa between 99 and 93.5 million years ago, between the Cenomanian and upper Turonian stages of the Late Cretaceous period. The genus was first identified from Egyptian relics found in 1912, and German palaeontologist Ernst Stromer described it in 1915. The original remnants were lost during World War II, but new information was discovered in the first decade of the twenty-first century. The fossils mentioned in the scientific literature are thought to represent one or two species. The most well-known species is S. aegyptiacus from Egypt, however S. maroccanus, a putative second species, has been found in Morocco. Some authors have also compared the modern spinosaurid genus Sigilmassasaurus to S. aegyptiacus, despite the fact that other scholars believe it to be a different taxon. The Brazilian genus Oxalaia from the Alcântara Formation is another potential junior synonym. Theropods like Tyrannosaurus, Giganotosaurus, and Carcharodontosaurus are other huge predators that are equivalent to Spinosaurus in size. Spinosaurus is the longest known terrestrial carnivore. The most current research reveals that earlier body size estimations are inaccurate and that S. aegyptiacus attained a length of 14 meters (46 feet) and a body mass of 7.4 tons (8.2 short tons). Similar to a modern crocodilian, Spinosaurus had a long, low, and thin cranium that contained straight conical teeth without serrations. Large, strong forelimbs with three-fingered hands and an expanded claw on the first digit would have been present. Although some authors have suggested that the spines were covered in fat and formed a hump, the distinctive neural spines of Spinosaurus were long extensions of the vertebrae (or backbones) that grew to a minimum length of 1.65 meters (5.4 ft). It is likely that skin connected the spines, forming a sail-like structure. Spinosaurus had smaller hip bones, and its legs were short in relation to its body. Tall, thin neural spines and lengthened chevrons enhanced the tail's long, narrow shape, giving it the appearance of a flexible paddle  Scientists generally agree that Spinosaurus hunted both terrestrial and aquatic animals and that it consumed fish. According to the available data, it was a highly semiaquatic animal that lived both on land and in water, similar to how current crocodilians do. Osteosclerosis (high bone density) in the leg bones of Spinosaurus allowed for improved buoyancy control, and the tail's paddle-like shape was probably utilized for subaquatic propulsion. The dorsal sail has been proposed to serve a variety of purposes, including thermoregulation and display, either to scare competitors or attract mates. It coexisted with several other dinosaurs, fish, crocodylomorphs, lizards, turtles, pterosaurs, and plesiosaurs on the humid tidal flats and mangrove forests.";
                cardImage.sprite     = spinoSprite;
            }
        }
    }

    // Clear scrollview
    public void DestroyChildren()
    {   

        while(rtTransform.childCount > 0)
        {
            DestroyImmediate(rtTransform.GetChild(0).gameObject);
        }

    }

    public void BackToCardList()
    {
        cardListMenu.SetActive(true);
        cardDetailMenu.SetActive(false);
        GetCards();
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
