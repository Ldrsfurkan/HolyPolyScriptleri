using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class HealthBar : MonoBehaviour
{
    private Slider slider;
    public TextMeshProUGUI healthCounter;
    private float currentHealth, maxHealth;
    void Awake()
    {   
        slider = GetComponent<Slider>();
    }

    
    void Update()
    {
       currentHealth = PlayerStatus.Instance.GetComponent<PlayerStatus>().currentHealth;
       maxHealth = PlayerStatus.Instance.GetComponent<PlayerStatus>().maxHealth;

       float fillValue = currentHealth / maxHealth;
       slider.value = fillValue;

       healthCounter.text = currentHealth + "/" + maxHealth;

    }
}
