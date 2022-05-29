using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : PoolableObject
{
    protected NavMeshAgent agent;
    protected Vector3 newPosition;
    public Animator anim;
    protected string[] tagToTarget = new string[] {"Zombie"};

    [SerializeField] protected bool debug = false;
    [SerializeField] protected float visionRange = 20;
    [SerializeField] protected float hearRange = 5;
    [SerializeField] protected float viewingAngle = 120;
    [SerializeField] protected Transform currentTarget = null;
    [SerializeField] protected float health = 10;
    [SerializeField] protected float damage;
    protected float wanderRange = 5.0f;
    [SerializeField] protected float zombieHealth = 10;
    protected List<GameObject> positionToLookFrom;
    GameObject killer;
    bool isControlled = false;

    protected bool canChange = true;
    private bool inTraining = false;
    private bool isTarget = false;

    [SerializeField]
    protected Zombie zombiePrefab;
    private ObjectPool objectPool;

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

        //if we have a target.. then keep looking at them!
        if (currentTarget && agent.enabled)
        {
            //we have a target - keep them in our eyes!

            Vector3 dir = currentTarget.position - transform.position;
            dir.y = 0;//This allows the object to only rotate on its y axis
            Quaternion rot = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, rot, 1f * Time.deltaTime);
            if( debug ) Debug.DrawRay(transform.position + Vector3.up * 1.5f, dir * dir.magnitude, Color.magenta);
        }
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

    protected virtual void OnEnable() {

        agent.enabled = true;
    }

    protected override void OnDisable() { 
        base.OnDisable();
        agent.enabled = false;
    }

    protected virtual void Awake()
    {
        positionToLookFrom = new List<GameObject>();
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        if(zombiePrefab) objectPool = ObjectPool.CreateInstance(zombiePrefab, 30);
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
    protected virtual void CheckHealth()
    {
        if (Health <= 0)
        {
            if (canChange)
            {
                humanState = HumanState.INFECTED;
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
            health = 1000; //invincible while we change
            canChange = false;
            FireArm firearm = GetComponent<FireArm>();
            if (firearm) firearm.enabled = false;
            
            agent.enabled = false;
            //animation
            anim.SetTrigger("dead");

            yield return new WaitForSeconds(3.5f);

            var zombie = objectPool.GetObject().GetComponent<Zombie>();
            zombie.transform.localPosition = transform.localPosition;
            zombie.transform.localRotation = transform.localRotation;
            gameObject.SetActive(false); //this npc is dead
            zombie.gameObject.SetActive(true); //the zombie lives
            zombie.agent.enabled = false;
            zombie.anim.SetTrigger("revive");
            zombie.StartReviveCooldown();
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
        if (agent.enabled)
            agent.SetDestination(newPosition);
    }

    

    protected virtual void SetTarget(Transform target)
    {
        this.currentTarget = target;
    }

    public float CalculatePathLength(Vector3 targetPosition)
    {
        // Create a path and set it based on a target position.
        NavMeshPath path = new NavMeshPath();
        if( !NavMesh.CalculatePath(transform.position, targetPosition, NavMesh.AllAreas, path) )
        {
            //error making path
            return float.PositiveInfinity;
        }

        if (path.status != NavMeshPathStatus.PathComplete)
        {
            //path couldnt actually reach destination
            return float.PositiveInfinity;
        }

        if (debug)
        {
            for (int i = 0; i < path.corners.Length - 1; i++)
                Debug.DrawLine(path.corners[i], path.corners[i + 1], Color.red);
        }

        // Create an array of points which is the length of the number of corners in the path + 2.
        Vector3[] allWayPoints = new Vector3[path.corners.Length + 2];

        // The first point is the enemy's position.
        allWayPoints[0] = transform.position;

        // The last point is the target position.
        allWayPoints[allWayPoints.Length - 1] = targetPosition;

        // The points inbetween are the corners of the path.
        for (int i = 0; i < path.corners.Length; i++)
        {
            allWayPoints[i + 1] = path.corners[i];
        }

        // Create a float to store the path length that is by default 0.
        float pathLength = 0;

        // Increment the path length by an amount equal to the distance between each waypoint and the next.
        for (int i = 0; i < allWayPoints.Length - 1; i++)
        {
            pathLength += Vector3.Distance(allWayPoints[i], allWayPoints[i + 1]);
        }

        return pathLength;
    }

    protected bool ScanForTarget()
    {
        GameObject foundTarget = null;
        float foundTargetDistance = 0;

        //first we get NPCs within our vision range
        int scanLayerMask = 1 << 3; // behind mask layer is where NPCs live
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, visionRange, scanLayerMask);

        foreach (var hitCollider in hitColliders)
        {
            //check what type this target is
            string targetTag = hitCollider.gameObject.tag;
            if (!Array.Exists(tagToTarget, s => s == targetTag) ) continue;

            Vector3 targetPosition = hitCollider.gameObject.transform.position;

            var localTarget = transform.InverseTransformPoint(targetPosition);
            // Use Trig to get my Angle IN RANGE 0 to 180
            float targetAngle = Mathf.Abs(Mathf.Atan2(localTarget.x, localTarget.z) * Mathf.Rad2Deg);
            
            Vector3 targetDirection = targetPosition - transform.position;
            float targetDistance = targetDirection.magnitude;

            //if the target is within our hearing range (even if behind) OR within our view angle
            if (targetDistance < hearRange || targetAngle < viewingAngle)
            {
                //possibly in line of sight - check no walls in the way..
                //Bit shift the index of the layer (8) to get a bit mask (walls)
                int wallLayerMask = 1 << 8;

                RaycastHit hit;
                // Does the ray intersect any walls
                if (Physics.Raycast(transform.position + Vector3.up * 1.5f, targetDirection, out hit, targetDistance, wallLayerMask))
                {
                    //hit a wall.. not our target!
                    if (debug) Debug.DrawRay(transform.position + Vector3.up * 1.5f, targetDirection * hit.distance, Color.red);
                }
                else
                {
                    //no walls. possible target
                    if (debug) Debug.DrawRay(transform.position + Vector3.up * 1.5f, targetDirection * targetDistance, Color.green);

                    //if first target or its closer than previous target then change to this
                    if (foundTarget == null || foundTargetDistance > targetDistance)
                    {
                        float distance = CalculatePathLength(hitCollider.transform.position);
                        //if we cant reach it and its not VERY far then set destination
                        if (distance != float.PositiveInfinity && distance < visionRange * 2)
                        {
                            foundTarget = hitCollider.gameObject;
                            foundTargetDistance = targetDistance;
                        }
                    }
                }
            }
            else
            {
                if (debug)
                {
                    //Debug.Log(targetAngle);
                    Debug.DrawRay(transform.position + Vector3.up * 1.5f, targetDirection, Color.blue);
                }
            }


        }

        if (foundTarget)
        {
            SetTarget(foundTarget.transform);
            return true;
        }

        SetTarget(null);
        return false;
    }

}
