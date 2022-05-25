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
    private bool isAttacking = false;

    protected override void Awake()
    {
        base.Awake();
        fireArm = GetComponent<FireArm>();
        transform.parent = GameObject.Find("Soldiers").transform;
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

            case IdleState.PATROLLING:
                //TODO Wander
                Wander();
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
            if (GetTargetDistance(currentTarget) <= attackRadius )
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
    }

    protected override void MoveToDestination()
    {
        base.MoveToDestination();
    }


    IEnumerator AttackCooldown()
    {

        if (ScanForTarget())
        {
            if (fireArm.canFire && GetTargetDistance(currentTarget) <= attackRadius)
            {
                isAttacking = true;
                fireArm.Fire(bulletTransform.position);
                fireArm.canFire = false;
                
                yield return new WaitForSeconds(fireArm.shotCooldown);

                if (currentTarget == null)
                {
                    isAttacking = false;
                    idleState = IdleState.PATROLLING;
                }

                fireArm.canFire = true;

            }
            else
            {
                isAttacking = false;
                fireArm.canFire = true;
                agent.SetDestination(new Vector3(currentTarget.position.x - attackRadius, transform.position.y, currentTarget.position.z - attackRadius));

            }
        }
        else
        {
            SetTarget(null);
        }

    }

    protected bool ScanForTarget()
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
                    if (hit.transform.tag == "Zombie")
                    {
                        Debug.Log("Hit a : " + hit.transform.name);
                        SetTarget(hit.transform);
                        return true;
                    }
                    
                }
            }
        }
        return false;
    }

}
