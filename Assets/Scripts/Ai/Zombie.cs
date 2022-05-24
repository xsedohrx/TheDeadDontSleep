using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Zombie : NPC
{
    [SerializeField] Transform currentTarget;
    Vector3 newPosition;
    protected NavMeshAgent agent;

    private float moveSpeed = 10f;
    private float attackCooldown = 1.0f;
    [SerializeField] float targetRange = 1.0f;
    [SerializeField] bool canAttack = true;
    [SerializeField] float damage;
    protected float attackRadius;
    protected float chaseRadius;
    public enum State { 
        WANDER,
        PERSUE,
        ATTACK
    }
    public State state;

    public static Action<float> DamageTarget;

    #region Unity Functions
    private void OnEnable(){ Projectile.OnTargetHit += TakeDamage;}
    private void OnDisable(){ Projectile.OnTargetHit -= TakeDamage;}


    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        humanState = HumanState.ZOMBIE;
        canChange = false;
        attackRadius = 1.5f;
        currentTarget = GameObject.FindGameObjectWithTag("Player").transform;
        SetTarget(currentTarget);

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

                break;

            case State.ATTACK:
                //StartCoroutine(AttackCooldown());
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

    private void MoveToTarget(Transform target)
    {        
        if (GetTargetDistance(target) > attackRadius)
        {
            Vector3 direction = new Vector3(target.position.x - transform.position.x, target.position.y - transform.position.y, target.position.z - transform.position.z);
            transform.Translate(new Vector3(direction.x, transform.position.y, direction.z) * Time.deltaTime);

        }
        else
        {
            state = State.ATTACK;
        }
    }

    #endregion

    protected override void TakeDamage(float damageToTake)
    {
        base.TakeDamage(damageToTake);
    }

    IEnumerator AttackCooldown() {
        if (canAttack && GetTargetDistance(currentTarget) < attackRadius)
        {
            canAttack = false;
            DamageTarget?.Invoke(damage);
            Debug.Log("Attacking");
            yield return new WaitForSeconds(attackCooldown);
            Debug.Log("Attacking complete");
            canAttack = true;

        }
    }

    void Wander() {
        if (scanForTarget())
        {
            state = State.PERSUE;
        }
        newPosition = new Vector3(
            UnityEngine.Random.Range(transform.position.x - 3, transform.position.x + 3),
            transform.position.y,
            UnityEngine.Random.Range(transform.position.x - 3, transform.position.x + 3)
            );
        agent.SetDestination(newPosition);

    }


    void Persue() { }



    //zombie vision
    public GameObject[] positionToLookFrom;
    bool scanForTarget()
    {
        for (int i = 0; i < positionToLookFrom.Length; i++)
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
                    if (hit.transform.tag == "Player")
                    {

                        return true;
                    }
                }
            }
        }
        return false;

    }


}
