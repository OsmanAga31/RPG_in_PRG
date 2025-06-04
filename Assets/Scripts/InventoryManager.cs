using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializeableList
{
    public List<Items> items;
    public List<int> amount;
}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    public Dictionary<Items, int> items { get; private set; }
    private SerializeableList saveItemsAndAmount = new();

    /// <summary>
    /// /////////////////////////// remove the dictionary and use only the serializeable list ///////////////////////////
    /// </summary>


    // Start is called before the first frame update
    void Start()
    {
        if (Instance == null)
        {
            Instance = this;

            LoadInventoryFromJson();
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void AddItem(Items item, int amount)
    {
        if (items == null)
        {
            items = new Dictionary<Items, int>();
        }

        if (items.ContainsKey(item))
        {
            items[item] += amount;
        }
        else
        {
            items.Add(item, amount);
        }
        SaveInventoryToJson();
    }

    public void AddItems(Dictionary<Items, int> newItems)
    {
        if (items == null)
        {
            items = new Dictionary<Items, int>();
        }
        foreach (KeyValuePair<Items, int> item in newItems)
        {
            AddItem(item.Key, item.Value);
        }
    }

    // save to json file with object name for the filename
    public void SaveInventoryToJson()
    {
        FillDictionaryToList(); // Fill the arrays from the dictionary before saving
        string json = JsonUtility.ToJson(saveItemsAndAmount, true);
        string filePath = Application.persistentDataPath + "/" + gameObject.name + "_inv.json";
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("Inventory saved to " + filePath + "\n\njson: " + json);
    }


    // load from json file with object name for the filename
    public void LoadInventoryFromJson()
    {
        string filePath = Application.persistentDataPath + "/" + gameObject.name + "_inv.json";
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
            Debug.Log("Inventory loaded from " + filePath);

            FillDictionaryItem(); // Fill the dictionary from the lists after loading

        }
        else
        {
            Debug.LogWarning("Inventory file not found: " + filePath);
        }
    }

    public void ListItems()
    {
        LoadInventoryFromJson();
        if (items == null || items.Count == 0)
        {
            Debug.Log("No items in the inventory.");
            return;
        }
        foreach (KeyValuePair<Items, int> item in items)
        {
            Debug.Log($"Item: {item.Key}, Amount: {item.Value}");
        }
        Debug.Log("Total items in inventory: " + items.Count + "\n\n\n");
    }

    public Dictionary<Items, int> GetInventory()
    {
        LoadInventoryFromJson();
        if (items == null)
        {
            items = new Dictionary<Items, int>();
        }
        return items;
    }

    private void FillDictionaryToList()
    {
        saveItemsAndAmount.items = new List<Items>(items.Keys);
        saveItemsAndAmount.amount = new List<int>(items.Values);
    }

    private void FillDictionaryItem()
    {
        items = new Dictionary<Items, int>();
        if (saveItemsAndAmount.items != null && saveItemsAndAmount.amount != null)
        {
            for (int i = 0; i < saveItemsAndAmount.items.Count; i++)
            {
                if (items.ContainsKey(saveItemsAndAmount.items[i]))
                {
                    items[saveItemsAndAmount.items[i]] += saveItemsAndAmount.amount[i];
                }
                else
                {
                    items.Add(saveItemsAndAmount.items[i], saveItemsAndAmount.amount[i]);
                }
            }
        }
    }

}
