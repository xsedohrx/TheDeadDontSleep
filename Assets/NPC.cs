using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    [SerializeField] private float health = 10;
    [SerializeField] private float zombieHealth = 10;
    protected float radius;
    protected bool canChange = true;
    public float Health { get { return health; } private set { } }

    public enum HumanState
    {
        ALIVE,
        TURNING,
        ZOMBIE,
        DEAD
    }
    protected HumanState humanState;

    //public static event Action<GameObject> OnDeath;

    protected virtual void Update() => GetHumanState(); 

    //Human state check
    void GetHumanState() {
        switch (humanState)
        {
            case HumanState.ALIVE:
                CheckHealth();
                break;
            case HumanState.TURNING:
                StartCoroutine(ChangeToZombie());
                break;
            case HumanState.ZOMBIE:
                DeathCheck();
                break;
            case HumanState.DEAD:
                Death();
                break;
        }
    }

    private void DeathCheck()
    {
        if (Health <= 0)
        {
            health = 0;
            Death();
        }
    }

    private void CheckHealth()
    {
        if (Health <= 0)
        {
            health = 0;
            humanState = HumanState.TURNING;
        }
    }

    IEnumerator ChangeToZombie() {
        if (canChange)
        {
            yield return new WaitForSeconds(.5f);
            canChange = false;
            health = zombieHealth;
            humanState = HumanState.ZOMBIE;
        }
    }

    public void Death() {
        //OnDeath?.Invoke(this.gameObject);
        GameObject.Destroy(gameObject);
    }
}
