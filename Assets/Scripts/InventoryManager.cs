using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SerializeableList
{
    public List<Items> items;
    public List<int> amount;

    public void Add(Items item, int amountToAdd)
    {
        if (items == null)
        {
            items = new List<Items>();
            amount = new List<int>();
        }
        items.Add(item);
        amount.Add(amountToAdd);
    }

    public void AddFromDictionary(Dictionary<Items, int> itemsToAdd)
    {
        if (itemsToAdd == null || itemsToAdd.Count == 0)
        {
            return;
        }
        if (items == null)
        {
            items = new List<Items>();
            amount = new List<int>();
        }
        foreach (var item in itemsToAdd)
        {
            Add(item.Key, item.Value);
        }
    }

    public Dictionary<Items, int> ToDictionary()
    {
        Dictionary<Items, int> dict = new Dictionary<Items, int>();
        if (items != null && amount != null && items.Count == amount.Count)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (dict.ContainsKey(items[i]))
                {
                    dict[items[i]] += amount[i];
                }
                else
                {
                    dict.Add(items[i], amount[i]);
                }
            }
        }
        return dict;
    }

    public void Clear()
    {
        if (items != null)
        {
            items.Clear();
        }
        if (amount != null)
        {
            amount.Clear();
        }
    }
    public int Count
    {
        get
        {
            if (items == null || amount == null || items.Count != amount.Count)
            {
                return 0;
            }
            return items.Count;
        }
    }

    public bool Contains(Items item)
    {
        return items != null && items.Contains(item);
    }
    public void Remove(Items item)
    {
        int index = items.IndexOf(item);
        if (index >= 0 && index < items.Count)
        {
            items.RemoveAt(index);
            amount.RemoveAt(index);
        }
    }
    public void RemoveAt(int index)
    {
        if (index < 0 || index >= items.Count)
        {
            throw new IndexOutOfRangeException("Index out of range for items list.");
        }
        items.RemoveAt(index);
        amount.RemoveAt(index);
    }
    public void ClearAll()
    {
        if (items != null)
        {
            items.Clear();
        }
        if (amount != null)
        {
            amount.Clear();
        }
    }
    public override string ToString()
    {
        if (items == null || amount == null || items.Count != amount.Count)
        {
            return "Empty Inventory";
        }
        string result = "Inventory:\n";
        for (int i = 0; i < items.Count; i++)
        {
            result += $"{items[i]}: {amount[i]}\n";
        }
        return result;
    }

}

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }
    //public Dictionary<Items, int> items { get; private set; }
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

    // add item to the serializable list, if item already exists, increase the amount
    public void AddItem(Items item, int amount)
    {     
        saveItemsAndAmount.Add(item, amount);
        SaveInventoryToJson();
    }

    public void AddItems(Dictionary<Items, int> newItems)
    {
        saveItemsAndAmount.AddFromDictionary(newItems);
        SaveInventoryToJson();
    }

    // save to json file with object name for the filename
    public void SaveInventoryToJson()
    {
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
            JsonUtility.FromJsonOverwrite(json, saveItemsAndAmount);
            Debug.Log("Inventory loaded from " + filePath);


        }
        else
        {
            Debug.LogWarning("Inventory file not found: " + filePath);
        }
    }

    public void ListItems()
    {
        LoadInventoryFromJson();
        if (saveItemsAndAmount != null && saveItemsAndAmount.Count > 0)
        {
            Debug.Log(saveItemsAndAmount.ToString());
        }
        else
        {
            Debug.Log("Inventory is empty.");
        }
    }

    public Dictionary<Items, int> GetInventory()
    {
        LoadInventoryFromJson();
        return saveItemsAndAmount.ToDictionary();
    }

}
