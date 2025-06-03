using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public Dictionary<SimpleBaseItem, int> items { get; private set; }


    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            //Load();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(SimpleBaseItem item, int amount)
    {
        if (items == null)
        {
            items = new Dictionary<SimpleBaseItem, int>();
        }

        if (items.ContainsKey(item))
        {
            items[item] += amount;
        }
        else
        {
            items.Add(item, amount);
        }
    }


}
