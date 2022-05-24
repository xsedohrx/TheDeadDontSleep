using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class NPC : MonoBehaviour
{
    protected NavMeshAgent agent;
    [SerializeField] protected float health = 10;
    [SerializeField] private float zombieHealth = 10;
    bool isControlled = false;
    
    protected bool canChange = true;
    private bool inTraining = false;
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
                CheckHealth();
                break;
            case HumanState.DEAD:
                Death();
                break;
        }
    }
    #endregion

    private void OnEnable()
    {
        Zombie.DamageTarget += TakeDamage;
    }

    private void OnDisable()
    {
        Zombie.DamageTarget -= TakeDamage;
    }

    protected virtual void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
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

            }
            else
            {
                Death();
            }
            health = 0;
        }
    }

    protected virtual IEnumerator ChangeToZombie() {
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

    protected virtual IEnumerator StartSoldierTraining()
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

    private void Death() {
        GameObject.Destroy(gameObject);
    }

    private void StartTraining() {
        humanState = HumanState.TRAINING;
    }

    private void TakeDamage(float damageToTake) {
        health -= damageToTake;

    }
}
