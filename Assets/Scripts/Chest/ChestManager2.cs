using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestManager2 : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool isOpen;
    // a dictionary with items and their amount
    [SerializeField] private int maxNumberOfIndividualItems; // Number of items to add to the chest
    [SerializeField] private int maxAmountOfItems; // Amount of items to add to the chest
    private int numberOfItems; // Number of items to add to the chest, can be set in the inspector
    public Dictionary<Items, int> items { get; private set; }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        LoadChestToJson();
        SetIsOpen(GetIsOpen()); // Ensure the chest state is set correctly on start
        fillChest(); // Fill the chest with items on start if not already open
    }

    private void fillChest() {
        if (!GetIsOpen())
        {
            // Initialize the items dictionary of the chestManager with some randomly named items and random amounts by using the Item enum and than adding it to the dictionary with the AddItem method from the ChestManager.cs.
            numberOfItems = Random.Range(1, maxNumberOfIndividualItems + 1); // Random number of items between 1 and maxNumberOfIndividualItems
            for (int i = 0; i<numberOfItems; i++)
            {
                // Generate a random item from the Item enum
                Items randomItem = (Items)Random.Range(0, System.Enum.GetValues(typeof(Items)).Length);
                int amount = Random.Range(1, maxAmountOfItems); // Random amount between 1 and maxAmountOfItems

                // Add the item to the chest
                AddItem(randomItem, amount);
            }
            ListItems(); // List items after adding for debugging purposes
            Debug.Log("Chest filled with items.\n\n");
        }
    }

    public void SetIsOpen(bool open)
    {
        animator.SetBool("Open", open);
        isOpen = open;
        //Debug.Log("Chest is now " + (isOpen ? "open" : "closed"));
        if (isOpen) // For testing
        {
            ListItems(); // List items when the chest is opened
        }
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

        foreach (KeyValuePair<Items, int> item in items)
        {
            Debug.Log($"Item: {item.Key}, Amount: {item.Value}");
        }
        Debug.Log("Total items in chest: " + items.Count);  
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
            items[item] = amount;
        }

        //Debug.Log($"Added {amount} of {item.itemName} to the chest. Total now: {items[item]}"); // for testing
    }

    // Remove every item and amount from chest and return it as dictionary to store it in another dictionary
    public Dictionary<Items, int> RemoveAllItems()
    {
        Dictionary<Items, int> removedItems = new Dictionary<Items, int>(items);
        items.Clear();
        //Debug.Log("Removed all items from the chest.");
        SaveChestToJson(); // Save the chest after removing all items
        return removedItems;
    }

    public Dictionary<Items, int> GetItemsAndRemove()
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
