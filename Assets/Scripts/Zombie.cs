using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : NPC
{
    [SerializeField] Transform target;
    private float moveSpeed = 10f;

    private void Awake()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;     
    }

    private void Start()
    {
        humanState = HumanState.ZOMBIE;
        canChange = false;
        radius = 1.5f;
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }

    protected override void Update()
    {
        base.Update();
        MoveToTarget(target);        
    }

    private void MoveToTarget(Transform target)
    {
        Vector2 direction = new Vector2(target.position.x - transform.position.x, target.position.y - transform.position.y);
        transform.Translate(direction * Time.deltaTime);
    }

    private void OnEnable()
    {
        Projectile.OnTargetHit += TakeDamage;
    }

    private void OnDisable()
    {
        Projectile.OnTargetHit -= TakeDamage;
    }

    void TakeDamage(float damageToTake) {
        health -= damageToTake;
    }

}
