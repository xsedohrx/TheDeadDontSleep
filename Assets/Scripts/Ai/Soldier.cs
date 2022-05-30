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

    
    [SerializeField]
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
                    agent.angularSpeed = 10; //dont deal with rotation while running
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

    protected override void OnEnable()
    {
        base.OnEnable();   
        fireArm = GetComponent<FireArm>();
        agent.stoppingDistance = 1.0f;
        damage = 2f;
        StartCoroutine(StartBehavior());
    }

    protected override void OnDisable(){ base.OnDisable();}
    protected override void Awake(){ 
        base.Awake();
        fireArm = GetComponent<FireArm>();
    }
    protected override void Start(){ base.Start();}
    protected override void MoveToDestination(){ 
        base.MoveToDestination();
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
                    if (agent.enabled) agent.SetDestination(transform.position + away.normalized * 7);
                }
                else
                {
                    idleState = IdleState.ATTACKING;
                    if (agent.enabled && agent.velocity.magnitude > 0)
                    {
                        //stop the agent! we are close enough
                        agent.SetDestination(transform.position);
                    }
                }

                if (fireArm.canFire)
                {
                    anim.SetTrigger("attack");
                    fireArm.Fire();

                    StartCoroutine(AttackCooldown());

                }
            }
            else
            {
                idleState = IdleState.ATTACKING;
                //not close enough to shoot .. moce closer
                if (agent.enabled) agent.SetDestination(currentTarget.position);

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
            if (agent.enabled) agent.SetDestination(transform.position);
            idleState = IdleState.PATROLLING;
        }

    }

    public void ResetStats()
    {
        agent.stoppingDistance = 1.0f;
        damage = 2f;
        health = 10;
        idleState = IdleState.PATROLLING;
        humanState = HumanState.ALIVE;
    }

}
