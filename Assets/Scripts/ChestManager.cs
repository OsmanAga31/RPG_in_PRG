using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool isOpen;
    // a dictionary with items and their amount
    public Dictionary<SimpleBaseItem, int> items { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        LoadChestToJson();
    }

    public void SetIsOpen(bool open)
    {
        animator.SetBool("Open", open);
        isOpen = open;
        //Debug.Log("Chest is now " + (isOpen ? "open" : "closed"));
        //if (isOpen) // For testing
        //{
        //    ListItems(); // List items when the chest is opened
        //}
    }

    public bool GetIsOpen()
    {
        return isOpen;
    }

    public void ListItems()
    {
        if (items == null || items.Count == 0)
        {
            Debug.Log("No items in the chest.");
            return;
        }

        foreach (KeyValuePair<SimpleBaseItem, int> item in items)
        {
            Debug.Log($"Item: {item.Key.itemName}, Amount: {item.Value}");
        }
        Debug.Log("Total items in chest: " + items.Count);  
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
            items[item] = amount;
        }

        //Debug.Log($"Added {amount} of {item.itemName} to the chest. Total now: {items[item]}"); // for testing
    }

    // Remove every item and amount from chest and return it as dictionary to store it in another dictionary
    public Dictionary<SimpleBaseItem, int> RemoveAllItems()
    {
        Dictionary<SimpleBaseItem, int> removedItems = new Dictionary<SimpleBaseItem, int>(items);
        items.Clear();
        //Debug.Log("Removed all items from the chest.");
        SaveChestToJson(); // Save the chest after removing all items
        return removedItems;
    }

    public Dictionary<SimpleBaseItem, int> GetItemsAndRemove()
    {
        SetIsOpen(true); // Open the chest before returning items
        return RemoveAllItems();
    }




    // save to json file with object name for the filename
    public void SaveChestToJson()
    {
        string json = JsonUtility.ToJson(this, true);
        string filePath = Application.persistentDataPath + "/" + gameObject.name + "_chest.json";
        System.IO.File.WriteAllText(filePath, json);
        Debug.Log("Chest saved to " + filePath);
    }
    // load from json file with object name for the filename
    public void LoadChestToJson() {
        string filePath = Application.persistentDataPath + "/" + gameObject.name + "_chest.json";
        if (System.IO.File.Exists(filePath))
        {
            string json = System.IO.File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(json, this);
            Debug.Log("Chest loaded from " + filePath);
        }
        else
        {
            Debug.LogWarning("Chest file not found: " + filePath);
        }
    }

    // for testing animation
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.CompareTag("Player"))
    //    {
    //        SetAnimatorValues(true);
    //    }
    //}
}
