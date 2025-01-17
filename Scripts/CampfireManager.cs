using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CampfireManager : MonoBehaviour
{
   public static CampfireManager Instance {get; set;}

    public Button cookButton;
    public Button exitButton;

    public GameObject foodSlot;
    public GameObject fuelSlot;

    public GameObject campfirePanel;
    public bool isUiOpen;
    public Campfire selectedCampfire;

    public CookingData cookingData;

    private void Awake() //Singleton Design Patern
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Update()
    {
        if(!FuelAndFoodAreValid())
        {
            cookButton.interactable = true;
        }
        else
        {
            cookButton.interactable = false;
        }
    }

    private bool FuelAndFoodAreValid()
    {
        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();
        InventoryItem food = foodSlot.GetComponentInChildren<InventoryItem>();

        if (fuel != null && food != null)
        {
            if(cookingData.validFuels.Contains(fuel.thisName) && 
                cookingData.validFoods.Any(CookableFood => CookableFood.name == food.thisName))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
        
    }

    public void CookButtonPressed()
    {
        InventoryItem food = foodSlot.GetComponentInChildren<InventoryItem>();
        selectedCampfire.StartCooking(food);

        InventoryItem fuel = fuelSlot.GetComponentInChildren<InventoryItem>();
        Destroy(fuel.gameObject);
        Destroy(food.gameObject);

        CloseUI();
    }

    public void OpenUI()
    {
        campfirePanel.SetActive(true);
        isUiOpen = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SelectionManager.Instance.DisableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;

        InventorySystem.Instance.OpenUI();
    }

    public void CloseUI()
    {
        campfirePanel.SetActive(false);
        isUiOpen = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SelectionManager.Instance.EnableSelection();
        SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
    }



}

