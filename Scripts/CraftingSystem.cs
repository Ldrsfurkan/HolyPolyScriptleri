using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CraftingSystem : MonoBehaviour
{
    public GameObject craftingScreenUI;
    public GameObject toolScreenUI;
    public GameObject survivalScreenUI;
    public bool isOpen;

    public List<string> inventoryItemList = new List<string>();

    //Category Buttons
    Button toolsBTN , survivalBTN;

    //Craft Buttons
    Button craftAxeBTN;
    Button craftPickaxeBTN;
    Button craftWatterBottleBTN;
    Button craftCampfireBTN;

    //Requirement Texts
    TextMeshProUGUI AxeReq1, AxeReq2, PickaxeReq1, PickaxeReq2, CFReq1, CFReq2, WBReq1, WBReq2;


    //Blueprints
    public ItemBlueprint AxeBLP = new ItemBlueprint("Axe",2,"Stone",3,"Stick",5);
    public ItemBlueprint PickaxeBLP = new ItemBlueprint("PickAxe",2,"Wood",5,"Stone",5);
    public ItemBlueprint WBBLP = new ItemBlueprint("WaterBottle",2,"Stone",1,"Stick",3);
    public ItemBlueprint CFBLP = new ItemBlueprint("CampFire",2,"Stone",4,"Wood",3);



   public static CraftingSystem Instance { get; set; }
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
        toolsBTN = craftingScreenUI.transform.Find("ToolsButton").GetComponent<Button>();
        toolsBTN.onClick.AddListener(delegate {OpenToolsCategory();});

        survivalBTN = craftingScreenUI.transform.Find("SurvivalButton").GetComponent<Button>();
        survivalBTN.onClick.AddListener(delegate {OpenSurvivalCategory();});

        //Axe
        AxeReq1 = toolScreenUI.transform.Find("Axe").transform.Find("AxeReq1").GetComponent<TextMeshProUGUI>();
        AxeReq2 = toolScreenUI.transform.Find("Axe").transform.Find("AxeReq2").GetComponent<TextMeshProUGUI>();
        craftAxeBTN =toolScreenUI.transform.Find("Axe").transform.Find("CraftButton").GetComponent<Button>();
        craftAxeBTN.onClick.AddListener(delegate {CraftAnyItem(AxeBLP); });

        //PickAxe 
        PickaxeReq1 = toolScreenUI.transform.Find("Pickaxe").transform.Find("PickaxeReq1").GetComponent<TextMeshProUGUI>();
        PickaxeReq2 = toolScreenUI.transform.Find("Pickaxe").transform.Find("PickaxeReq2").GetComponent<TextMeshProUGUI>();
        craftPickaxeBTN =toolScreenUI.transform.Find("Pickaxe").transform.Find("CraftButton").GetComponent<Button>();
        craftPickaxeBTN.onClick.AddListener(delegate {CraftAnyItem(PickaxeBLP); });

        //WaterBottle
        WBReq1 = survivalScreenUI.transform.Find("WaterBottle").transform.Find("WBReq1").GetComponent<TextMeshProUGUI>();
        WBReq2 = survivalScreenUI.transform.Find("WaterBottle").transform.Find("WBReq2").GetComponent<TextMeshProUGUI>();
        craftWatterBottleBTN =survivalScreenUI.transform.Find("WaterBottle").transform.Find("CraftButton").GetComponent<Button>();
        craftWatterBottleBTN.onClick.AddListener(delegate {CraftAnyItem(WBBLP); });
        //CampFire
        CFReq1 = survivalScreenUI.transform.Find("CampFire").transform.Find("CFReq1").GetComponent<TextMeshProUGUI>();
        CFReq2 = survivalScreenUI.transform.Find("CampFire").transform.Find("CFReq2").GetComponent<TextMeshProUGUI>();
        craftCampfireBTN =survivalScreenUI.transform.Find("CampFire").transform.Find("CraftButton").GetComponent<Button>();
        craftCampfireBTN.onClick.AddListener(delegate {CraftAnyItem(CFBLP); });

    }
    
    void OpenToolsCategory()
    {
        craftingScreenUI.SetActive(false);
        toolScreenUI.SetActive(true);
        
    }
    void OpenSurvivalCategory()
    {
        craftingScreenUI.SetActive(false);
        survivalScreenUI.SetActive(true);
    }
    void CraftAnyItem(ItemBlueprint blueprintToCraft)
    {
        //envantere ekle
    InventorySystem.Instance.AddToInventory(blueprintToCraft.itemName);

        //resourceleri envanterden kaldır
    if(blueprintToCraft.numOfRequirements == 1)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.req1Amount);
        }
        else if(blueprintToCraft.numOfRequirements == 2)
        {
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req1, blueprintToCraft.req1Amount);
            InventorySystem.Instance.RemoveItem(blueprintToCraft.req2, blueprintToCraft.req2Amount);
        }

        //envanter listesini düzelt
    StartCoroutine(CheckEnvanter());

    }

    public IEnumerator CheckEnvanter() //Recalculatelist delay verme
    {
        yield return 0; //no delay
        InventorySystem.Instance.ReCalculateList();
        RefreshNeededItems();
    }

   
    void Update()
    {

         if (Input.GetKeyDown(KeyCode.C) && !isOpen)
        {
            craftingScreenUI.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            isOpen = true;
            SelectionManager.Instance.DisableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = false;
        }
        else if (Input.GetKeyDown(KeyCode.C) && isOpen)
        {
            craftingScreenUI.SetActive(false);
            toolScreenUI.SetActive(false);
            survivalScreenUI.SetActive(false);

            if(!InventorySystem.Instance.isOpen)
            {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SelectionManager.Instance.EnableSelection();
            SelectionManager.Instance.GetComponent<SelectionManager>().enabled = true;
            }

            isOpen = false;
        }
    }
    public void RefreshNeededItems()
    {
        int stone_count = 0;
        int stick_count = 0;
        int wood_count = 0;
        inventoryItemList = InventorySystem.Instance.itemList;

        foreach (string itemName in inventoryItemList)
        {
            switch (itemName)   
            {
                case "Stone":
                    stone_count +=1;
                    break;
                case "Stick":
                    stick_count +=1;
                    break;
                case "Wood":
                    wood_count +=1;
                    break;
                
            }
        }
    
    //   ---- AXE ---- //
   AxeReq1.text = "3 Stone [" + stone_count + "]"; 
   AxeReq2.text = "5 Stick [" + stick_count + "]"; 

   if(stone_count >=3 && stick_count >= 5)
   {
    craftAxeBTN.gameObject.SetActive(true);
   }
   else
   {
    craftAxeBTN.gameObject.SetActive(false);
   }
    //   ---- AXE ---- //

    //   ---- Pickaxe ---- //
   PickaxeReq1.text = "5 Wood [" + wood_count + "]"; 
   PickaxeReq2.text = "5 Stone [" + stone_count + "]"; 

   if(stone_count >=5 && wood_count >= 5)
   {
    craftPickaxeBTN.gameObject.SetActive(true);
   }
   else
   {
    craftPickaxeBTN.gameObject.SetActive(false);
   }
    //   ---- Pickaxe ---- //
    
    //   ---- WaterBottle ---- //
   WBReq1.text = "1 Stone [" + stone_count + "]"; 
   WBReq2.text = "3 Stick [" + stick_count + "]"; 

   if(stone_count >=1 && stick_count >= 3)
   {
    craftWatterBottleBTN.gameObject.SetActive(true);
   }
   else
   {
    craftWatterBottleBTN.gameObject.SetActive(false);
   }
   //   ---- WatterBottle---- //
    //   ---- CampFire ---- //
   CFReq1.text = "4 Stone [" + stone_count + "]"; 
   CFReq2.text = "3 Wood [" + wood_count + "]"; 

   if(stone_count >=4 && wood_count >= 3)
   {
    craftCampfireBTN.gameObject.SetActive(true);
   }
   else
   {
    craftCampfireBTN.gameObject.SetActive(false);
   }
    //   ---- CampFire ---- //
 
   }

}
