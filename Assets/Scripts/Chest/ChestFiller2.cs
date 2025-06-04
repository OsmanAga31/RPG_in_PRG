using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestFiller2 : MonoBehaviour
{
    private GameObject chest;
    


    void Start()
    {
        chest = gameObject;
        if (chest != null)
        {
            
        }
        else
        {
            Debug.LogError("Chest GameObject not found in the scene.");
        }

    }
}
