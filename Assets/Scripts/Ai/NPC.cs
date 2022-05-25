using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    protected NavMeshAgent agent;
    protected Vector3 newPosition;
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

    protected virtual void SetTarget(Transform target)
    {
        this.currentTarget = target;
    }

}
