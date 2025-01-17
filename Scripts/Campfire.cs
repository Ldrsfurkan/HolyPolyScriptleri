using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    public bool playerInRange;
    public bool isCooking;
    public float cookingTimer;
    public CookableFood foodBeingCooked;
    public string readyFood;
    public GameObject cookedFood;

    void Update()
    {
        float distance = Vector3.Distance(PlayerStatus.Instance.playerBody.transform.position, transform.position);

        if(distance < 10f)
        {
            playerInRange = true;
        }
        else
        {
            playerInRange = false;
        }

        if(isCooking)
        {
            cookingTimer -= Time.deltaTime;
        }
        if(cookingTimer <= 0 && isCooking)
        {
            Debug.Log("pişti");
            isCooking = false;
            readyFood = GetCookedFood(foodBeingCooked);
        }
    }

    private string GetCookedFood(CookableFood food)
    {
       return food.cookedFoodName;
    }

    public void OpenUI()
    {
        CampfireManager.Instance.OpenUI();
        CampfireManager.Instance.selectedCampfire = this;


        if(readyFood != "")
        {
            GameObject rf = Instantiate(Resources.Load<GameObject>(cookedFood.name),
                CampfireManager.Instance.foodSlot.transform.position,
                CampfireManager.Instance.foodSlot.transform.rotation);

            rf.transform.SetParent(CampfireManager.Instance.foodSlot.transform);
        }
    }

    public void StartCooking(InventoryItem food)
    {
        foodBeingCooked = ConvertIntoCookable(food);
        isCooking = true;
        cookingTimer = TimeToCookFood(foodBeingCooked);
    }

    private CookableFood ConvertIntoCookable(InventoryItem food)
    {
        foreach(CookableFood cookable in CampfireManager.Instance.cookingData.validFoods)
        {
            if(cookable.name == food.thisName)
            {
                return cookable;
            }
        }
        return new CookableFood();
    }

    private float TimeToCookFood(CookableFood food)
    {
        return food.timeToCook;
    }
}
