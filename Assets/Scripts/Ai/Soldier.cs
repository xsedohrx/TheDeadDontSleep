using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soldier : NPC
{
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
    private float fireRate = .5f;

    protected override void Awake()
    {
        base.Awake();
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
                    if (Vector3.Distance(currentTarget.position, transform.position) <= 5)
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
        agent.SetDestination(newPosition);
    }

    IEnumerator AttackCooldown()
    {
        yield return new WaitForSeconds(fireRate);
    }

    

}
