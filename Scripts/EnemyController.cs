using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    //Skeleton
    [SerializeField] float health = 20f;
    [SerializeField] float skeletonAttackCD = 1f;
    [SerializeField] float skeletonAttackRange = 2f;
    [SerializeField] float skeletonAggroRange = 8f;

    private GameObject player;
    private Animator anim;
    private NavMeshAgent agent;
    private float timePassed;
    private int hitCount=0;
    private float newDestinationCD = 0.5f;
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
       SkeletonAttack();
    }
    public void TakeDamage(float damageAmount)
    {
        health -=damageAmount;
        anim.SetTrigger("damage");

        if(health <= 0)
        {
            Die();
        }
    }
    void Die()
    {
        Destroy(gameObject);
    }
    void SkeletonAttack()
    {
         anim.SetFloat("speed", agent.velocity.magnitude / agent.speed);

        if(timePassed >= skeletonAttackCD)
        {
            if(Vector3.Distance(player.transform.position, transform.position) <= skeletonAttackRange)
            {
                anim.SetTrigger("attack");
                timePassed = 0;
            }
        }
        timePassed += Time.deltaTime;

        if(newDestinationCD <= 0 && Vector3.Distance(player.transform.position, transform.position) <= skeletonAggroRange)
        {
            newDestinationCD = 0.5f;
            agent.SetDestination(player.transform.position);
        }
        newDestinationCD -= Time.deltaTime;
        transform.LookAt(player.transform);

    }
    public void StartDealDamage()
    { 
        hitCount++;
        GetComponentInChildren<EnemyDamageDealer>().StartDealDamage();
        if(hitCount == 3) // sonra bakarız bi kere oldumu hep oluyo düzeltiriz
        {
            skeletonAttackCD = 3f;
            hitCount = 0;
        }
    }
    public void EndDealDamage()
    {
        GetComponentInChildren<EnemyDamageDealer>().EndDealDamage();
    }

}
