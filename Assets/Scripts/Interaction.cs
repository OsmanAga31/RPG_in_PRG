using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.InputSystem.InputAction;

public class Interaction : MonoBehaviour
{
    [SerializeField] private GameObject player; // Reference to the player GameObject
    private GameObject interactable; // Reference to the interactable object

    //private void OnEnable()
    //{
    //    playerInput.actions["Interact"].performed += Interact; // Subscribe to the Interact action
    //}

    //private void OnDisable()
    //{
    //    playerInput.actions["Interact"].performed -= Interact; // Unsubscribe from the Interact action
    //}

    //public void Interact()//InputAction.CallbackContext ctx)
    //{
    //    Debug.Log("Interact action performed"); // for testing
    //    //if (ctx.performed)
    //    //{
    //    // Check if the player is near a interactable object/chest and open/use it
    //    if (interactable != null && interactable.CompareTag("Chest") && !interactable.GetComponent<ChestManager>().GetIsOpen())
    //    {
    //    //interacltable.GetComponent<ChestManager>().GetItemsAndRemove(); // Open the chest and get the Items
    //    Debug.Log("Chest is: " + interactable.GetComponent<ChestManager>().GetIsOpen()); // for testing
    //    }
    //    //}
    //}

    public void Interact(CallbackContext ctx)
    {
        if (!ctx.started) return; // Only proceed if the action was performed
        Debug.Log($"Interact action {ctx.phase}"); // for testing

        interactable = player.GetComponent<BaseCharacterController>().GetInteractable(); // Get the interactable object from the player controller


        // Check if the player is near an interactable object/chest and open/use it
        if (interactable != null && interactable.CompareTag("Chest") && !interactable.GetComponent<ChestManager2>().GetIsOpen())
        {
            // get inventorymanager instance and add items to it
            InventoryManager.Instance.AddItems(interactable.GetComponent<ChestManager2>().GetItemsAndRemove()); // Open the chest and get the Items
            Debug.Log("Chest is: " + interactable.GetComponent<ChestManager2>().GetIsOpen()); // for testing
            interactable = null; // Reset interactable after interaction
        }
        else if (interactable != null && interactable.CompareTag("Chest") && interactable.GetComponent<ChestManager2>().GetIsOpen())
        {
            Debug.Log("Chest is already open"); // for testing
        }
        else
        {
            Debug.Log("No interactable object found or chest is already open"); // for testing
        }
    }
}
