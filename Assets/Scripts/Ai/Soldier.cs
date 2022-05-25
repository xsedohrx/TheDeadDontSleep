using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : NPC
{
    private float attackRadius = 10;
    bool destinationReached = false;
    Vector3 newDestination;
    [SerializeField] float patrolTimer = 2f;
    public enum IdleState { 
        IDLE,
        PATROLLING,
        SEARCHING,
        CHASING,
        ATTACKING
    }

    public IdleState idleState;
    private FireArm fireArm;
    [SerializeField] private Transform bulletTransform;

    protected override void Awake()
    {
        base.Awake();
        fireArm = GetComponent<FireArm>();
        gameObject.tag = "Soldier";
        agent.stoppingDistance = 3.5f;
        damage = 2f;
    }

    protected override void Start()
    {
        base.Start();        
        StartCoroutine(StartBehavior());
    }

    protected override void Update(){ base.Update(); }

    IEnumerator StartBehavior()
    {

        WaitForSeconds wait = new WaitForSeconds(patrolTimer);

        while (enabled)
        {
            StateSwitch();
            yield return wait;

        }
    }

    private void StateSwitch()
    {
        switch (idleState)
        {
            case IdleState.IDLE:
                if (ScanForTarget())
                {
                    if (Vector3.Distance(currentTarget.position, transform.position) <= attackRadius)
                    {
                        idleState = IdleState.ATTACKING;
                    }
                    else
                    {
                        idleState = IdleState.PATROLLING;
                    }
                }
                else
                {
                    idleState = IdleState.PATROLLING;
                }

                break;

            case IdleState.PATROLLING:
                Wander();
                break;

            case IdleState.SEARCHING:
                break;

            case IdleState.CHASING:
                break;

            case IdleState.ATTACKING:
                StartCoroutine(AttackCooldown());
                break;
        }
    }

    private void Wander() {
       MoveToDestination();
        if (ScanForTarget())
        {
            idleState = IdleState.ATTACKING;
        }
        else
        {
            idleState = IdleState.IDLE;
        }
    }

    protected override void MoveToDestination()
    {
        base.MoveToDestination();
    }


    IEnumerator AttackCooldown()
    {

            float timer = 0;
            if (timer <= fireArm.shotCooldown)
            {
                Debug.Log(timer + "Timer");
                timer++;
                fireArm.canFire = false;
            }
            else {
                Debug.Log(timer + "BANG");
                fireArm.Fire(bulletTransform.position);

                yield return new WaitForSeconds(fireArm.shotCooldown);
                timer = 0;
                fireArm.canFire = true;
            }
        
    }    

}
