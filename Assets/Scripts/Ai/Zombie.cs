using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : NPC
{
    List<GameObject> positionToLookFrom;
    [SerializeField] Transform currentTarget = null;
    Vector3 newPosition;

    private float wanderSpeed = 3.5f;
    private float chaseSpeed = 6f;
    private float attackCooldown = 1.0f;
    private float wanderRange = 8.0f;
    
    [SerializeField] bool canAttack = true;
    [SerializeField] float damage;

    public enum State { 
        WANDER,
        PERSUE,
        ATTACK
    }
    public State state;

    public static Action<float> DamageTarget;

    #region Unity Functions


    protected override void Awake()
    {
        base.Awake();
        positionToLookFrom = new List<GameObject>();
        gameObject.tag = "Zombie";
        agent.stoppingDistance = 3.5f;

    }

    private void Start()
    {
        foreach (RayScript ray in gameObject.GetComponentsInChildren<RayScript>())
        {
            positionToLookFrom.Add(ray.gameObject);
        }

        humanState = HumanState.ZOMBIE;
        canChange = false;
        


    }

    protected override void Update()
    {
        base.Update();
        StateSwitch();
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
    private void SetTarget(Transform target)
    {
        this.currentTarget = target;
    }

    private float GetTargetDistance(Transform target) {
        return Vector3.Distance(target.position, transform.position);
    }


    #endregion


    IEnumerator AttackCooldown() {
        if (scanForTarget())
        {
            if (canAttack && GetTargetDistance(currentTarget) < agent.stoppingDistance)
            {
                canAttack = false;
                DamageTarget?.Invoke(damage);
                Debug.Log("Attacking");
                yield return new WaitForSeconds(attackCooldown);
                Debug.Log("Attacking complete");
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
        if (scanForTarget())
        {
            state = State.PERSUE;
        }
        agent.speed = wanderSpeed;
        MoveToDestination();

    }

    private void MoveToDestination()
    {
        newPosition = new Vector3(
            UnityEngine.Random.Range(transform.position.x - wanderRange, transform.position.x + wanderRange),
            transform.position.y,
            UnityEngine.Random.Range(transform.position.x - wanderRange, transform.position.x + wanderRange)
            );
        agent.SetDestination(newPosition);
    }


    void Persue() {
        if (GetTargetDistance(currentTarget) < agent.stoppingDistance)
        {
            state = State.ATTACK;
        }
        else
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(currentTarget.position);
        }        
    }



    //zombie vision
    bool scanForTarget()
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
                        currentTarget = hit.transform;
                        return true;
                    }
                    else
                    {
                        currentTarget = null;
                    }
                }
            }
        }
        return false;

    }


}
