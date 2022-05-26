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
        ATTACKING,
        RETREAT
    }

    private IdleState _idleState;
    public IdleState idleState
    {
        get
        {
            return _idleState;
        }
        set
        {
            _idleState = value;
            agent.speed = 3.5f;
            agent.angularSpeed = 9000;
            switch (value)
            {
                case IdleState.RETREAT:
                    agent.speed = 2f; //slower running away
                    agent.angularSpeed = 90; //dont deal with rotation while running
                    break;
                case IdleState.PATROLLING:
                    SetTarget(null);
                    break;
            }
        }
    }

    private FireArm fireArm;
    [SerializeField] private Transform bulletTransform;
    private bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        fireArm = GetComponent<FireArm>();
        //transform.parent = GameObject.Find("Soldiers").transform;
        gameObject.tag = "Soldier";
        agent.stoppingDistance = 1.0f;
        damage = 2f;
    }

    protected override void Start()
    {
        base.Start();        
        StartCoroutine(StartBehavior());
    }

    protected override void Update(){ 
        base.Update();

        //check if we can fire here otherwise we will only ever fire every 2 seconds!
        if (currentTarget)
        {

            var currentRange = GetTargetDistance(currentTarget);
            if (currentRange <= attackRadius)
            {
                if (currentRange <= 5)
                {
                    //run away !!
                    //Debug.Log("Run away !!!");
                    idleState = IdleState.RETREAT;
                    Vector3 away = transform.position - currentTarget.position;
                    away.y = 0;
                    agent.SetDestination(transform.position + away.normalized * 7);
                }
                else
                {
                    idleState = IdleState.ATTACKING;
                    if (agent.velocity.magnitude > 0)
                    {
                        //stop the agent! we are close enough
                        agent.SetDestination(transform.position);
                    }
                }

                if (fireArm.canFire)
                {
                    fireArm.Fire();

                    StartCoroutine(AttackCooldown());

                }
            }
            else
            {
                idleState = IdleState.ATTACKING;
                //not close enough to shoot .. moce closer
                agent.SetDestination(currentTarget.position);

            }

        }

    }

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

            case IdleState.PATROLLING:
                //TODO Wander
                Wander();
                break;

            case IdleState.RETREAT:
            case IdleState.ATTACKING:
                CheckAttack();
                break;
        }
    }

    private void Wander() {
        MoveToDestination();
        if (ScanForTarget())
        {
            idleState = IdleState.ATTACKING;
        }
    }

    protected override void MoveToDestination()
    {
        base.MoveToDestination();
    }


    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        fireArm.canFire = false;

        yield return new WaitForSeconds(fireArm.shotCooldown);

        isAttacking = false;
        fireArm.canFire = true;
    }

    protected void CheckAttack()
    {
        if (ScanForTarget())
        {
        }
        else
        {
            //lost our target
            agent.SetDestination(transform.position);
            idleState = IdleState.PATROLLING;
        }

    }

}
