using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : NPC
{
    [SerializeField] Transform target;
    private float moveSpeed = 10f;

    #region Unity Functions
    private void OnEnable()
    {
        Projectile.OnTargetHit += TakeDamage;
    }

    private void OnDisable()
    {
        Projectile.OnTargetHit -= TakeDamage;
    }

    private void Awake()
    {
        //TODO Change this to find any player when needed (probably be an action?)
        FindPlayerTarget();
    }

    private void Start()
    {
        humanState = HumanState.ZOMBIE;
        canChange = false;
        radius = 1.5f;
        //gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    protected override void Update()
    {
        base.Update();
        MoveToTarget(target);        
    }

    #endregion
    #region Target Functions
    private void FindPlayerTarget()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void MoveToTarget(Transform target)
    {
        Vector3 direction = new Vector3(target.position.x - transform.position.x, target.position.y - transform.position.y, target.position.z - transform.position.z);
        transform.Translate(direction * Time.deltaTime);
    }

    #endregion

    void TakeDamage(float damageToTake) {
        health -= damageToTake;

    }



}
