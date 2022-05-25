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
        transform.parent = GameObject.Find("Zombies").transform;
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

    protected virtual bool ScanForTarget()
    {
        for (int i = 0; i < positionToLookFrom.Count; i++)
        {
            Ray[] raysForSearch = new Ray[3];

            Vector3 noAngle = positionToLookFrom[i].transform.forward;
            Quaternion spreadAngle = Quaternion.AngleAxis(-20, new Vector3(0, 1, 0));
            Vector3 negativeDirection = spreadAngle * noAngle;
            spreadAngle = Quaternion.AngleAxis(20, new Vector3(0, 1, 0));
            Vector3 positiveDirection = spreadAngle * noAngle;

            Debug.DrawLine(positionToLookFrom[i].transform.position, positionToLookFrom[i].transform.position + noAngle * 10.0f, Color.red);
            Debug.DrawLine(positionToLookFrom[i].transform.position, positionToLookFrom[i].transform.position + positiveDirection * 10.0f, Color.red);
            Debug.DrawLine(positionToLookFrom[i].transform.position, positionToLookFrom[i].transform.position + negativeDirection * 10.0f, Color.red);

            raysForSearch[0] = new Ray(positionToLookFrom[i].transform.position, noAngle);
            raysForSearch[1] = new Ray(positionToLookFrom[i].transform.position, negativeDirection);
            raysForSearch[2] = new Ray(positionToLookFrom[i].transform.position, positiveDirection);

            foreach (Ray ray in raysForSearch)
            {
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, 20))
                {
                    if (hit.transform.tag == "Player" || hit.transform.tag == "Human")
                    {
                        SetTarget(hit.transform);
                        return true;
                    }
                    else
                    {
                        SetTarget(null);
                    }
                }
            }
        }
        return false;

    }
}
