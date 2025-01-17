using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class WaterBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI waterCounter;
    private float currentWater, maxWater;
    void Awake()
    {   
        slider = GetComponent<Slider>();
    }

    void Update()
    {
       currentWater = PlayerStatus.Instance.GetComponent<PlayerStatus>().currentWater;
       maxWater = PlayerStatus.Instance.GetComponent<PlayerStatus>().maxWater;

       float fillValue = currentWater/ maxWater;
       slider.value = fillValue;

       waterCounter.text = currentWater + "%";

    }
}
