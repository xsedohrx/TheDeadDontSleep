using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Vector3 newPosition;
    private Animator anim;

    [SerializeField] protected Transform currentTarget = null;
    [SerializeField] protected float health = 10;
    [SerializeField] protected float damage;
    protected float wanderRange = 5.0f;
    [SerializeField] private float zombieHealth = 10;
    protected List<GameObject> positionToLookFrom;
    GameObject killer;
    bool isControlled = false;

    protected bool canChange = true;
    private bool inTraining = false;

    private bool isTarget = false;  

    public bool IsTarget
    {
        get { return isTarget; }
        set { isTarget = value; }
    }


    public float Health { get { return health; } private set { } }
    protected float GetTargetDistance(Transform target) { return Vector3.Distance(target.position, transform.position); }

    protected void UpdateAnimator()
    {
        if (!anim || transform.tag == "Player") return;

        anim.SetFloat("velocityZ", agent.velocity.z / agent.speed);
        anim.SetFloat("velocityX", agent.velocity.x / agent.speed);
    }

    #region Human State Variables

    public enum HumanState
    {
        ALIVE,
        TRAINING,
        INFECTED,
        ZOMBIE,
        SOLDIER,
        DEAD
    }
    public HumanState humanState;

    #endregion
    #region Human State Switch
    //Human state check
    void GetHumanState() {
        UpdateAnimator();
        switch (humanState)
        {
            case HumanState.ALIVE:
                CheckHealth();
                break;
            case HumanState.TRAINING:
                StartCoroutine(StartSoldierTraining());
                break;
            case HumanState.INFECTED:
                StartCoroutine(ChangeToZombie());
                break;
            case HumanState.ZOMBIE:
                CheckHealth();
                break;
            case HumanState.SOLDIER:
                //TODO Make soldier
                //TrainingComplete();
                //CheckHealth();
                break;
            case HumanState.DEAD:
                Death();
                break;
        }
    }
    #endregion


    protected virtual void Awake()
    {
        positionToLookFrom = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
    }

    protected virtual void Start()
    {
        foreach (RayScript ray in gameObject.GetComponentsInChildren<RayScript>())
        {
            positionToLookFrom.Add(ray.gameObject);
        }

    }

    protected virtual void Update() => GetHumanState();

    //Check to see if npc should change or die!
    private void CheckHealth()
    {
        if (Health <= 0)
        {
            if (canChange)
            {
                humanState = HumanState.INFECTED;
                Destroy(gameObject.GetComponent<PlayerMotor>());

            }
            else
            {
                Death();
            }
            health = 0;
        }
    }

    private IEnumerator ChangeToZombie() {
        if (canChange)
        {
            yield return new WaitForSeconds(.5f);
            canChange = false;
            health = zombieHealth;
            humanState = HumanState.ZOMBIE;
            gameObject.AddComponent<Zombie>();
            Destroy(this);
        }
    }

    private IEnumerator StartSoldierTraining()
    {
        if (!inTraining)
        {
            inTraining = true;
            yield return new WaitForSeconds(.5f);
            inTraining = false;
            humanState = HumanState.SOLDIER;
            gameObject.AddComponent<Soldier>();
            Destroy(this);
        }
    }

    protected virtual void Death() {
        
        GameObject.Destroy(gameObject);
    }

    private void StartTraining() {
        humanState = HumanState.TRAINING;
    }

    public void TakeDamage(float damageToTake) {
        health -= damageToTake;

    }

    void SetKiller(GameObject killer) {
        if (Health<=0) {
            this.killer = killer;
            killer.GetComponent<Zombie>().isControlled = true;
                Debug.Log(killer.GetComponent<Zombie>().isControlled + "Zombie! ");
        }
    }

    protected virtual void MoveToDestination()
    {
        newPosition = new Vector3(
            UnityEngine.Random.Range(transform.position.x - wanderRange, transform.position.x + wanderRange),
            transform.position.y,
            UnityEngine.Random.Range(transform.position.x - wanderRange, transform.position.x + wanderRange)
            );
        agent.SetDestination(newPosition);
    }

    

    protected virtual void SetTarget(Transform target)
    {
        this.currentTarget = target;
    }

}
