using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;
using System;

public class SelectionManager : MonoBehaviour
{
    // Singleton Design Patern.
    public static SelectionManager Instance {get; set;}


    public bool onTarget;
    public GameObject selectedObject;
    public GameObject interaction_Info_UI;
    private TextMeshProUGUI interaction_text;

    public Image crosshair;
    public Image handIcon;
    public bool handVisible;
    public GameObject selectedCampfire;


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

    private void Start()
    {
        onTarget = false;
        interaction_text = interaction_Info_UI.GetComponent<TextMeshProUGUI>();
    }
 
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selectionTransform = hit.transform;

            InteractableObject interactable = selectionTransform.GetComponent<InteractableObject>();
           
            if (interactable && interactable.playerInRange)
            {
                onTarget = true;
                selectedObject = interactable.gameObject;
                interaction_text.text = interactable.GetItemName();
                interaction_Info_UI.SetActive(true);
                crosshair.gameObject.SetActive(false);
                handIcon.gameObject.SetActive(true);
            }
                /*if(interactable.CompareTag("Pickable"))
                {
                    crosshair.gameObject.SetActive(false);
                    handIcon.gameObject.SetActive(true);
                    handVisible = true;
                }
                else
                {
                    crosshair.gameObject.SetActive(true);
                    handIcon.gameObject.SetActive(false);
                    handVisible = false;
                }
            }
            else //hit var ama interactable obje değil
            { 
                onTarget = false;
                interaction_Info_UI.SetActive(false);
                crosshair.gameObject.SetActive(true);
                handIcon.gameObject.SetActive(false);
                handVisible = false;
            }*/
        Campfire campfire = selectionTransform.GetComponent<Campfire>();
        
        if(campfire && campfire.playerInRange && PlacementSystem.Instance.inPlacementMode == false)
        {
            interaction_text.text = "Interact";
            interaction_Info_UI.SetActive(true);

            selectedCampfire = campfire.gameObject;

            if(Input.GetMouseButtonDown(0) && campfire.isCooking == false)
            {
                campfire.OpenUI();
            }
            else
            {
                if(selectedCampfire != null)
                {
                    selectedCampfire = null;
                }
            }
            
        }



    Animal animal = selectionTransform.GetComponent<Animal>();

    if(animal && animal.playerInRange)
    {
        if(animal.isDead)
        {
            interaction_text.text = "Loot";
            interaction_Info_UI.SetActive(true);

            crosshair.gameObject.SetActive(false);
            handIcon.gameObject.SetActive(true);
            handVisible = true;

            if(Input.GetKeyDown(KeyCode.E))
            {
                Lootable lootable = animal.GetComponent<Lootable>();
                Loot(lootable);
            }
        }
        else
        {
            interaction_text.text = animal.animalName;
            interaction_Info_UI.SetActive(true);
            crosshair.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);
            handVisible = false;

            if(Input.GetMouseButtonDown(0)) //&& EquipSystem.Instance.isHoldingWeapon)
            {
                StartCoroutine(DealDamageTo(animal, 0.3f, EquipSystem.Instance.GetWeaponDamage()));
            }
        
            }
        if(!interactable && !animal && !campfire)
        {
            onTarget = false;
            handVisible = false;

            crosshair.gameObject.SetActive(true);
            handIcon.gameObject.SetActive(false);

            interaction_text.text = "";
            interaction_Info_UI.SetActive(false);
        }
    }
    }
    else //hit var ama interactable obje değil
    { 
        onTarget = false;
        interaction_Info_UI.SetActive(false);
        crosshair.gameObject.SetActive(true);
        handIcon.gameObject.SetActive(false);
        handVisible = false;
    }
        
    }

    private void Loot(Lootable lootable)
    {
        if(lootable.wasLootCalculated == false)
        {
            List<LootRecieved> recievedLoot = new List<LootRecieved>();

            foreach (LootPossibility loot in lootable.possibleLoot)
            {
                var lootAmount = UnityEngine.Random.Range(loot.amountMin, loot.amountMax+1);
                if(lootAmount != 0)
                {
                    LootRecieved lt = new LootRecieved();
                    lt.item = loot.item;
                    lt.amount = lootAmount;
                    recievedLoot.Add(lt);
                }
            }

            lootable.finalLoot = recievedLoot;
            lootable.wasLootCalculated = true;
        }
        foreach(LootRecieved lootRecieved in lootable.finalLoot)
        {
            for (int i =0; i<lootRecieved.amount; i++)
            {
                InventorySystem.Instance.AddToInventory("Meat");
            }
        }
        Destroy(lootable.gameObject);     
    }

    IEnumerator DealDamageTo(Animal animal, float delay, int damage)
    {
        Debug.Log("DealDamageTO ya girdi0");
        yield return new WaitForSeconds(delay);
        animal.TakeDamage(damage);
    }

    public void DisableSelection()
    {
        handIcon.enabled = false;
        crosshair.enabled = false;
        interaction_Info_UI.SetActive(false);
        selectedObject = null;
    }
    public void EnableSelection()
    {
        handIcon.enabled = true;
        crosshair.enabled = true;
        interaction_Info_UI.SetActive(true);
    }

   
   
}