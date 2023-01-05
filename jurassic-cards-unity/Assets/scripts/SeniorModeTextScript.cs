using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SeniorModeTextScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.GetInt("seniorMode", 0) == 1)
        {
            Text[] textObjects = FindObjectsOfType<Text>(true);

            foreach (Text text in textObjects)
            {
                int maxsize = System.Math.Max(text.fontSize, 45);
                text.fontSize = System.Math.Min(text.fontSize + 10, maxsize);
            }

            Canvas.ForceUpdateCanvases();
        }
    }
}
