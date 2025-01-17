using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
public class PlacementSystem : MonoBehaviour
{
    public static PlacementSystem Instance { get; set; }
 
    public GameObject placementHoldingSpot; 
    public GameObject enviromentPlaceables;
 
 
    public bool inPlacementMode;
    [SerializeField] bool isValidPlacement;
 
    [SerializeField] GameObject itemToBePlaced;
    public GameObject inventoryItemToDestory;
 
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }
 
    public void ActivatePlacementMode(string itemToPlace)
    {
        GameObject item = Instantiate(Resources.Load<GameObject>(itemToPlace));
 
        // İsmini değiştir (clone)
        item.name = itemToPlace;
        item.transform.SetParent(placementHoldingSpot.transform, false);
        itemToBePlaced = item;
        // Construction mode aktive et
        inPlacementMode = true;
    }
 
 
 
    private void Update()
    {
 
        if (inPlacementMode)
        {
           // Display UI for player,
        }
        else
        {
            // Disable UI
        }
 
        if (itemToBePlaced != null && inPlacementMode)
        {
            if (IsCheckValidPlacement())
            {
                isValidPlacement = true;
                //itemToBePlaced.GetComponent<PlacebleItem>().SetValidColor();
            }
            else
            {
                isValidPlacement = false;
                //itemToBePlaced.GetComponent<PlacebleItem>().SetInvalidColor();
            }
        }
 
        if (Input.GetMouseButtonDown(0) && inPlacementMode && isValidPlacement)
        {
            PlaceItemFreeStyle();
            DestroyItem(inventoryItemToDestory);
        }
 
        // Cancel Placement                     //Daha sonra ui itemi silme ekleriz 
        if (Input.GetKeyDown(KeyCode.X))
        {
            inventoryItemToDestory.SetActive(true);
            inventoryItemToDestory = null;
            DestroyItem(itemToBePlaced);
            itemToBePlaced = null;
            inPlacementMode = false;
        }
    }
 
    private bool IsCheckValidPlacement()
    {
        if (itemToBePlaced != null)
        {
            return itemToBePlaced.GetComponent<PlacebleItem>().isValidToBeBuilt;
        } 
 
        return false;
    }
 
    private void PlaceItemFreeStyle()
    {
        itemToBePlaced.transform.SetParent(enviromentPlaceables.transform, true);
 
        //itemToBePlaced.GetComponent<PlacebleItem>().SetDefaultColor();
        itemToBePlaced.GetComponent<PlacebleItem>().enabled = false;
 
        itemToBePlaced = null;
 
        inPlacementMode = false;
    }
 
    private void DestroyItem(GameObject item)
    {
        DestroyImmediate(item);
        InventorySystem.Instance.ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
}