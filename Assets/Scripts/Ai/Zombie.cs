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
    [SerializeField] private bool isAttacking;
    [SerializeField] bool canAttack = true;
    public FMODUnity.StudioEventEmitter attackEmitter;

    public enum State { 
        WANDER,
        PERSUE,
        ATTACK
    }
    public State state;

    #region Unity Functions

    protected override void OnEnable()
    {
        base.OnEnable();
        humanState = HumanState.ZOMBIE;
        canChange = false;
        StartCoroutine(StartBehavior());
    }

    protected override void OnDisable(){ base.OnDisable(); }
    protected override void Start(){ base.Start(); }

    protected override void Awake()
    {
        base.Awake();

        gameObject.tag = "Zombie";
        agent.stoppingDistance = 1f;

        tagToTarget = new string[] { "Human", "Soldier", "Player" };
        damage = 2;
    }

    protected override void Update()
    {
        base.Update();

        //check if we can fire here otherwise we will only ever fire every 2 seconds!
        if (currentTarget && GetTargetDistance(currentTarget) <= 1.15)
        {
            if (agent.enabled && agent.velocity.magnitude > 0)
            {
                //stop the agent! we are close enough
                agent.SetDestination(transform.position);
            }

            if (canAttack)
            {
                StartCoroutine(AttackCooldown());

            }
        }

    }


    IEnumerator StartBehavior()
    {
        WaitForSeconds wait = new WaitForSeconds(updateSpeed);
        while (enabled)
        {
            StateSwitch();            
            yield return wait;
            //Debug.Log("Testing");
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
        yield return new WaitForSeconds(3.5f);
        agent.enabled = true;
    }

    IEnumerator AttackCooldown()
    {
        isAttacking = true;
        canAttack = false;

        currentTarget.GetComponent<NPC>().TakeDamage(damage);

        attackEmitter.Play();
        //GameObject.FindGameObjectWithTag("AudioManager").GetComponent<FmodPlayer>().PlaySound("event:/TheDeadDontSleep/Zombie/ZombieBite");
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


}
