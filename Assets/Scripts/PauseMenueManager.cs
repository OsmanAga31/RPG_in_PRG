using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;
using UnityEngine.UI;

public class PauseMenueManager : MonoBehaviour
{
    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private GameObject inventoryItemsListUI;
    [SerializeField] private GameObject itemPrefab;
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
            ClearInventoryItemsList();
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
                // child 0 is the background, child 1 is the item icon, child 2 is the item name, child 3 is the item amount
                itemObj.transform.GetChild(2).GetComponent<TMP_Text>().SetText(item.Key.ToString());
                itemObj.transform.GetChild(3).GetComponent<TMP_Text>().SetText(item.Value.ToString() + "x");
            }
            else
            {
                Debug.LogWarning("ItemUI component not found on the item prefab.");
            }
        }
    }
}
