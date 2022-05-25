using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : NPC
{


    private float wanderSpeed = 3.5f;
    private float chaseSpeed = 6f;
    private float attackCooldown = 1.0f;
    private float sightRadius = 8.0f;
    public float updateSpeed = .5f;
    private bool isAttacking;
    [SerializeField] bool canAttack = true;
    

    public enum State { 
        WANDER,
        PERSUE,
        ATTACK
    }
    public State state;

    #region Unity Functions


    protected override void Awake()
    {
        base.Awake();

        gameObject.tag = "Zombie";
        agent.stoppingDistance = 3.5f;

    }

    protected override void Start()
    {
        base.Start();
        humanState = HumanState.ZOMBIE;
        canChange = false;        
        StartCoroutine(StartBehavior());
    }
    protected override void Update(){ base.Update(); }


    IEnumerator StartBehavior()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);
        while (enabled)
        {
            StateSwitch();            
            yield return wait;
            Debug.Log("Testing");
        }
    }

    private void StateSwitch()
    {
        switch (state)
        {
            case State.WANDER:
                Wander();
                break;

            case State.PERSUE:
                Persue();
                break;

            case State.ATTACK:
                StartCoroutine(AttackCooldown());
                break;
        }
    }

    #endregion
    #region Target Functions
    protected override void SetTarget(Transform target){ base.SetTarget(target); }
    private float GetTargetDistance(Transform target) { return Vector3.Distance(target.position, transform.position); }

    #endregion
    
    IEnumerator AttackCooldown() {
        if (ScanForTarget())
        {            
            if (canAttack && GetTargetDistance(currentTarget) <= agent.stoppingDistance)
            {
                isAttacking = true;
                
                canAttack = false;
                currentTarget.GetComponent<NPC>().TakeDamage(damage);
                if (currentTarget == null)
                {
                    isAttacking = false;
                    state = State.WANDER;
                }
                yield return new WaitForSeconds(attackCooldown);                
                canAttack = true;

            }
            else
            {
                isAttacking = false;
                canAttack = true;
                
            }
        }
        else
        {
            state = State.WANDER;
        }
        
    }

    void Wander()
    {
        if (ScanForTarget())
        {
            state = State.PERSUE;
        }
        agent.speed = wanderSpeed;
        MoveToDestination();

    }

    protected override void MoveToDestination()
    {
        base.MoveToDestination();
    }


    void Persue() {
        if (GetTargetDistance(currentTarget) < agent.stoppingDistance)
        {
            state = State.ATTACK;
        }
        else
        {
            currentTarget.GetComponent<NPC>().IsTarget = true;
            agent.speed = chaseSpeed;
            agent.SetDestination(currentTarget.position);
        }        
    }



    //zombie vision
    protected override bool ScanForTarget()
    {
        return base.ScanForTarget();
    }

}
