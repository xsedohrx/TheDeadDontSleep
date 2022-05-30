using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : NPC
{


    private float wanderSpeed = 1f;
    private float chaseSpeed = 7f;
    private float attackCooldown = 1.0f;
    private float sightRadius = 8.0f;
    public float updateSpeed = .5f;
    [SerializeField] private bool isAttacking;
    [SerializeField] bool canAttack = true;
    

    public enum State { 
        WANDER,
        PERSUE,
        ATTACK,
        DYING
    }
    public State state;

    #region Unity Functions

    protected override void OnEnable()
    {
        base.OnEnable();
        humanState = HumanState.ZOMBIE;
        state = State.WANDER;
        canChange = false;
        StartCoroutine(StartBehavior());
        gameObject.tag = "Zombie";
        agent.stoppingDistance = 1f;
        tagToTarget = new string[] { "Human", "Soldier", "Player" };
        damage = 2;
        health = zombieHealth;
        GetComponent<CapsuleCollider>().enabled = true;
        anim.SetTrigger("reset");
        GameManager.instance.AddToZombies(gameObject);
    }

    protected override void OnDisable(){
        GameManager.instance.RemoveFromZombies(gameObject);
        base.OnDisable(); 
    }
    protected override void Start(){ base.Start(); }

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Update()
    {
        base.Update();

        //check if we can fire here otherwise we will only ever fire every 2 seconds!
        if (currentTarget && GetTargetDistance(currentTarget) <= 1.25)
        {
            /*
            if (agent.enabled && agent.velocity.magnitude > 0)
            {
                //stop the agent! we are close enough
                agent.SetDestination(transform.position);
            }
            */

            if (canAttack && state != State.DYING)
            {
                StartCoroutine(AttackCooldown());

            }
        }

    }

    protected override void Death()
    {
        StartCoroutine(DeathBehavior());
        
    }

    IEnumerator DeathBehavior()
    {
        health = 10000; // so we dont keep triggerring death
        state = State.DYING;
        anim.SetTrigger("dead");
        agent.enabled = false;
        gameObject.tag = "Untagged";
        GetComponent<CapsuleCollider>().enabled = false;
        yield return new WaitForSeconds(10);
        gameObject.SetActive(false);
    }

    IEnumerator StartBehavior()
    {
        while (enabled)
        {
            StateSwitch();            
            yield return new WaitForSeconds(updateSpeed);
            //Debug.Log("Testing");
        }
    }

    private void StateSwitch()
    {
        switch (state)
        {
            case State.WANDER:
                updateSpeed = 1.5f;
                Wander();
                break;

            case State.PERSUE:
                updateSpeed = 0.2f;
                Persue();
                break;

            case State.ATTACK:
                updateSpeed = 0.2f;
                CheckAttack();
                break;
        }
    }

    #endregion
    #region Target Functions
    protected override void SetTarget(Transform target){ base.SetTarget(target); }
    private float GetTargetDistance(Transform target) { return Vector3.Distance(target.position, transform.position); }

    #endregion

    protected void CheckAttack()
    {
        if (ScanForTarget())
        {
            if (GetTargetDistance(currentTarget) <= agent.stoppingDistance + 0.05)
            {
                //stop the agent! we are close enough
                if(agent.enabled) agent.SetDestination(transform.position);
            }
            else
            {
                //not close enough to shoot .. move closer
                state = State.PERSUE;
            }
        }
        else
        {
            //lost our target
            if(agent.enabled) agent.SetDestination(transform.position);
            SetTarget(null);
            state = State.WANDER;
        }

    }

    public void StartReviveCooldown()
    {
        StartCoroutine(ReviveCooldown());
    }

    IEnumerator ReviveCooldown()
    {
        yield return new WaitForSeconds(1.5f);
        if(state != State.DYING) agent.enabled = true;
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        canAttack = false;

        currentTarget.GetComponent<NPC>().TakeDamage(damage);
        anim.SetTrigger("attack");
        yield return new WaitForSeconds(attackCooldown);

        isAttacking = false;
        canAttack = true;
    }

    void Wander()
    {
        if(agent.enabled) agent.speed = wanderSpeed;
        MoveToDestination();
        if (ScanForTarget())
        {
            state = State.PERSUE;
        }
    }

    protected override void MoveToDestination()
    {
        base.MoveToDestination();
    }


    void Persue() {
        if (!ScanForTarget())
        {
            state = State.WANDER;
        }
        else if (GetTargetDistance(currentTarget) <= agent.stoppingDistance + 0.05)
        {
            state = State.ATTACK;
        }
        else if(GetTargetDistance(currentTarget) > visionRange)
        {
            state = State.WANDER;
        }
        else
        {
            currentTarget.GetComponent<NPC>().IsTarget = true;
            agent.speed = chaseSpeed;
            if(agent.enabled) agent.SetDestination(currentTarget.position);
        }        
    }

    public void ResetStats()
    {
        agent.stoppingDistance = 1.0f;
        damage = 2f;
        health = 10;
        state = State.WANDER;
        canAttack = true;
        humanState = HumanState.ALIVE;

    }

}
