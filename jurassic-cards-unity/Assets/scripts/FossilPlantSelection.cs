using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FossilPlantSelection : MonoBehaviour
{
    public int fossilID;

    public void SelectFossil()
    {
        FindObjectOfType<FossilController>().selectedFossilID = fossilID;
        FindObjectOfType<HomePageController>().OpenFossilInfo();
    }
}
