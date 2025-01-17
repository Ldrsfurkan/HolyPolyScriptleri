using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class InteractableObject : MonoBehaviour
{
    public bool playerInRange;
    public string itemName;
    private KeyCode pickUpButton = KeyCode.E;
    //Toplanabilir eşyalardaki 2.collider isTrigger on , 30,10,30 gibi 
    public string GetItemName()
    {
        return itemName;
    }
    void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
    void Update()
    {
        if(Input.GetKeyDown(pickUpButton) && playerInRange && SelectionManager.Instance.onTarget && SelectionManager.Instance.selectedObject == gameObject)
        {
            if(!InventorySystem.Instance.CheckIfFull())
            {
                InventorySystem.Instance.AddToInventory(itemName);
                Destroy(gameObject);
            }
            else
            {
                Debug.Log("inventory full");
            }
           
        }
    }
}