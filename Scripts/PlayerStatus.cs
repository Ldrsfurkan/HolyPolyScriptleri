using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class PlayerStatus : MonoBehaviour
{
        //--- Health ---//
    public float currentHealth;
    public float maxHealth;

        //--- Food ---//
    public float currentFood;
    public float maxFood;
    float distanceTraveled = 0;
    Vector3 lastPosition;
    public GameObject playerBody;

        //--- Water ---//
    public float currentWater;
    public float maxWater;
    public bool isPlayerDead;
    public GameObject reaspawnCheckpoint;
    public GameObject deadWarning;

    public static PlayerStatus Instance { get; set;}
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
        currentHealth = maxHealth;
        currentFood = maxFood;
        currentWater = maxWater;
        StartCoroutine(WaterSpending());
    }
    void Update()
    {
        FoodSpending();
        RespawnPlayer();
    }
    void FoodSpending()
    {
        distanceTraveled += Vector3.Distance(playerBody.transform.position, lastPosition);
        lastPosition = playerBody.transform.position;

        if(distanceTraveled >=10)
        {
            distanceTraveled=0;
            currentFood -=1;
        }
    }
    IEnumerator WaterSpending()
    {
        while(true)
        {
            currentWater -= 1;
            yield return new WaitForSeconds(5);
        }
    }
      /*public void TakeDamage(float damageAmount)
    {
        currentHealth-= damageAmount;
        if(currentHealth <= 0 && !isPlayerDead)
        {
            Die();
        }
    }*/
    public void Die()
    {
        isPlayerDead = true;
        deadWarning.SetActive(true);
        RespawnPlayer();
    }
    public void setFood(float newfood)
    {
        currentFood = newfood;
    }
    public void setHydration(float newwater)
    {
        currentWater = newwater;
    }
    public void setHealth(float newhealth)
    {
        currentHealth = newhealth;
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        if(currentHealth <= 0 && !isPlayerDead)
        {
            Die();
        }
    }

    public void RespawnPlayer()
    {
        if(Input.GetKeyDown(KeyCode.F) && isPlayerDead)
        {
        StartCoroutine(RespawnCourutine());
        }
    }
    public IEnumerator RespawnCourutine()
    {
        playerBody.GetComponent<FirstPersonController>().enabled = false;
        playerBody.GetComponent<CharacterController>().enabled = false;

        Vector3 position = reaspawnCheckpoint.transform.position;
        playerBody.transform.position = position;

        yield return new WaitForSeconds(0.2f);

        isPlayerDead = false;

        playerBody.GetComponent<FirstPersonController>().enabled = true;
        playerBody.GetComponent<CharacterController>().enabled = true;

        currentHealth = maxHealth;
        currentFood = maxFood;
        currentWater = maxWater;
        
        deadWarning.SetActive(false);
    }

    
}
