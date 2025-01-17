using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FoodBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI foodCounter;
    private float currentFood, maxFood;
    void Awake()
    {   
        slider = GetComponent<Slider>();
    }

    void Update()
    {
       currentFood = PlayerStatus.Instance.GetComponent<PlayerStatus>().currentFood;
       maxFood = PlayerStatus.Instance.GetComponent<PlayerStatus>().maxFood;

       float fillValue = currentFood/ maxFood;
       slider.value = fillValue;

       foodCounter.text = currentFood + "/" + maxFood;

    }
}
