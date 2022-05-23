using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : NPC
{
    [SerializeField] Transform target;
    private float moveSpeed = 10f;
    private float attackCooldown = 1.0f;
    [SerializeField] float targetRange = 1.0f;
    [SerializeField] bool canAttack = true;
    [SerializeField] float damage;

    public static Action<float> DamageTarget;

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
        gameObject.GetComponent<Renderer>().material.color = Color.red;
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
        if (Vector3.Distance(target.position, transform.position) <= radius)
        {
            StartCoroutine(AttackCooldown());
        }
    }

    #endregion

    void TakeDamage(float damageToTake) {
        health -= damageToTake;

    }

    IEnumerator AttackCooldown() {
        if (canAttack)
        {
            canAttack = false;
            Debug.Log("Attacking");
            yield return new WaitForSeconds(attackCooldown);
            DamageTarget?.Invoke(damage);
            Debug.Log("Attacking complete");
            canAttack = true;
        }
    }



}
