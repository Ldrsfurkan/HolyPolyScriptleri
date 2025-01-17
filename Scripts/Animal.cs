using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.PostProcessing;

public class Animal : MonoBehaviour
{
    public string animalName;
    public bool playerInRange;
    [SerializeField] int currentHealth;
    [SerializeField] int maxHealth;
    Animator anim;
    public bool isDead;

    private void Start()
    {
        currentHealth = maxHealth;
        anim = GetComponent<Animator>();
    }

    public void TakeDamage(int damage)
    {
        if(isDead == false)
        {
            currentHealth -=damage;

            if(currentHealth <= 0)
            {
                anim.SetBool("Death",true);
                GetComponent<NavMeshAgent>().enabled = false;
                isDead = true;
            }
        }
        else
        {
            Debug.Log("öldü");
        }
    }
     void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

}
