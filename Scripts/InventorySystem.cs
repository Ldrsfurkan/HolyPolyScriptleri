using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
 
public class InventorySystem : MonoBehaviour
{
    public static InventorySystem Instance { get; set; }
    public GameObject inventoryScreenUI;
    public GameObject ItemInfoUI;
    public bool isOpen;
    private KeyCode inventoryButton = KeyCode.I;
    public List<GameObject> slotList = new List<GameObject>();
    public List<string> itemList = new List<string>();
    private GameObject itemToAdd;
    private GameObject whatSlotToEquip;

    //PickUp PopUp
    public GameObject pickupAlert;
    public TextMeshProUGUI pickupName;
    public Image pickupImage;
 
 
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
 
 
    void Start()
    {
        isOpen = false;
        SetSlotList();
        Cursor.visible = false;

    }
 
 
    void Update()
    {
 
        if (Input.GetKeyDown(inventoryButton) && !isOpen)
        {
            OpenUI();
        }
        else if (Input.GetKeyDown(inventoryButton) && isOpen)
        {
            CloseUI();
        }
    }
    void SetSlotList()
    {
        foreach (Transform child in inventoryScreenUI.transform)
        {
            if(child.CompareTag("InventorySlot"))
            {
                slotList.Add(child.gameObject);
            }
        }
    } 

    public void OpenUI()
    {
         inventoryScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            isOpen = true;
            Cursor.visible = true;
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
    }
    public void CloseUI()
    {
         inventoryScreenUI.SetActive(false);
            
            if(!CraftingSystem.Instance.isOpen && !CampfireManager.Instance.isUiOpen)
            {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }
            isOpen = false;
    }
















    public void AddToInventory(string itemName)
    {
        whatSlotToEquip = FindNextEmptySlot();
        itemToAdd = Instantiate(Resources.Load<GameObject>(itemName), whatSlotToEquip.transform.position, whatSlotToEquip.transform.rotation);
        itemToAdd.transform.SetParent(whatSlotToEquip.transform);

        itemList.Add(itemName);

        PickupPopUp(itemName, itemToAdd.GetComponent<Image>().sprite);

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
    private GameObject FindNextEmptySlot()
    {
        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount == 0)
            {
                return slot;
            }
        }
        return new GameObject(); // full olsa zaten bu kod çalışmayacak hata vermesin diye yazdım.
    }

    public bool CheckIfFull()
    {
        int counter = 0;

        foreach (GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                counter +=1;
            } 
        }

        if(counter == 21)
            {
                return true;
            }
            else
            {
                return false;
            }
    }

    public void RemoveItem(string nameToRemove, int amountToRemove)
    {
        int counter = amountToRemove;

        for(var i = slotList.Count - 1; i >=0; i--)
        {
            if(slotList[i].transform.childCount>0)
            {
                if(slotList[i].transform.GetChild(0).name == nameToRemove + "(Clone)" && counter !=0)
                {
                    Destroy(slotList[i].transform.GetChild(0).gameObject);

                    counter-=1;
                }
            }

        }

        ReCalculateList();
        CraftingSystem.Instance.RefreshNeededItems();
    }
    public void ReCalculateList()
    {
        itemList.Clear();
        foreach(GameObject slot in slotList)
        {
            if(slot.transform.childCount > 0)
            {
                string name = slot.transform.GetChild(0).name;
                string str1 = "(Clone)";
                string result = name.Replace(str1,""); // Stone (Clone)

                itemList.Add(result);
            }
        }
    }
    void PickupPopUp(string itemName, Sprite itemSprite)
    {
        pickupAlert.SetActive(true);
        pickupName.text = itemName;
        pickupImage.sprite = itemSprite;

        StartCoroutine(HidePickupAlert());
    }
    private IEnumerator HidePickupAlert() 
    {
        yield return new WaitForSeconds(2f);
        pickupAlert.SetActive(false);
        
       
    }
}