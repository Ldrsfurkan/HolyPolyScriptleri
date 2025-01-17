using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageDealer : MonoBehaviour
{
  
    private bool canDealDamage;
    private bool hasDealtDamage;
    [SerializeField] float weaponLength;
    [SerializeField] int weaponDamage;
    void Start()
    {
        canDealDamage = false;
        hasDealtDamage = false;
    }

    void Update()
    {
        if (canDealDamage && !hasDealtDamage)
        {
            RaycastHit hit;
            int layerMask = 1 << 7; // player katmanı 7 de

            if(Physics.Raycast(transform.position, -transform.up, out hit, weaponLength, layerMask))
            {
                Debug.Log("damaged");
                PlayerStatus.Instance.TakeDamage(weaponDamage);
                hasDealtDamage = true; 
                
            }
        }
    }
    public void StartDealDamage()
    {
        canDealDamage = true;
        hasDealtDamage = false;
    }
    public void EndDealDamage()
    {
        canDealDamage = false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position - transform.up * weaponLength);
    }
}