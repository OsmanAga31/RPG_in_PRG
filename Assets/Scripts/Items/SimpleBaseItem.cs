using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SimpleBaseItem
{
    [SerializeField] public string itemName;
    [SerializeField] public string itemDescription;

    public SimpleBaseItem(string itemName, string itemDescription)
    {
        this.itemName = itemName;
        this.itemDescription = itemDescription;
    }

}


