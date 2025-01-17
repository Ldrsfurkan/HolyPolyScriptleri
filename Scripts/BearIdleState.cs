using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearIdleState : StateMachineBehaviour
{
    float timer;
    float idleTime = 4f;

    Transform player;
    public float detectionAreaRadius = 18f;


    //OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer = 0f;
       player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Walk State 'e gecis
       timer += Time.deltaTime;

       if(timer > idleTime)
       {
        animator.SetBool("isWalking", true);
       }

       // Chase State gecis
       float distanceFromPlayer = Vector3.Distance(player.position, animator.transform.position);
       if(distanceFromPlayer < detectionAreaRadius)
       {
        animator.SetBool("isChasing",true);
       }

    }
    //OnStateExit aç ses falan ekliceksen.




}
