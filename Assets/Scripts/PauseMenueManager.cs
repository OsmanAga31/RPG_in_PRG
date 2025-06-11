using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;
using System.IO;
using UnityEngine.Windows;
using UnityEngine.WSA;

public class PauseMenueManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject inventoryItemsListUI;
    [SerializeField] private GameObject itemPrefab;
    [SerializeField] private List<string> backgroundImage;
    private InventoryManager inventoryManager;
    private BaseCharacterController baseCC;
    private Dictionary<Items, int> inventoryItems = new Dictionary<Items, int>();

    private void Start()
    {
        baseCC = FindObjectOfType<BaseCharacterController>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void TogglePauseMenu(CallbackContext ctx)
    {
        if (!ctx.performed) return; // Only proceed if the action was performed
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
        baseCC.PausePlayer(pauseMenuUI.activeSelf);

        // if the pause menu is active, clear the inventoryItemsList and refill it with the items prefab
        if (pauseMenuUI.activeSelf)
        {
            FillInventoryItemsList();
        }
    }

    private void ClearInventoryItemsList()
    {
        foreach (Transform child in inventoryItemsListUI.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void FillInventoryItemsList()
    {
        // clear the inventory items list UI first and then get the inventory items from the inventory manager
        ClearInventoryItemsList();
        inventoryItems = inventoryManager.GetInventory();

        //inventoryManager.ListItems(); // For debugging, list items in the console
        Debug.Log("Filling inventory items list with " + inventoryItems.Count + " items.");

        // Iterate through the inventory items and create a new item prefab for each item
        foreach (KeyValuePair<Items, int> item in inventoryItems)
        {
            GameObject itemObj = Instantiate(itemPrefab, inventoryItemsListUI.transform);
            if (itemObj != null)
            {
                // set background image with random background image from the list
                if (backgroundImage.Count > 0)
                {
                    string randomBackground = backgroundImage[Random.Range(0, backgroundImage.Count)];
                    itemObj.transform.GetChild(0).GetComponent<Image>().sprite = Resources.Load<Sprite>("invIconsBackground/" + randomBackground);
                }

                // set item icon
                itemObj.transform.GetChild(1).GetComponent<Image>().sprite = Resources.Load<Sprite>("invIcons/" + item.Key.ToString()); // Only learned with A.I. that the images must be in the Resources folder

                // set item name and amount
                itemObj.transform.GetChild(2).GetComponent<TMP_Text>().SetText(item.Key.ToString());
                itemObj.transform.GetChild(4).GetComponent<TMP_Text>().SetText(item.Value.ToString());

                // set item description 
                itemObj.transform.GetChild(5).GetChild(1).GetComponent<TMP_Text>().SetText($"Description of item \"{item.Key.ToString()}\" coming soon!");

            }
            else
            {
                Debug.LogWarning("ItemUI component not found on the item prefab.");
            }
        }
    }
}
