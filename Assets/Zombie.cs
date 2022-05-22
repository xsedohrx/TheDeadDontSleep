using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : NPC
{
    [SerializeField] Transform target;
    private float moveSpeed = 10f;
    
    private void Start()
    {
        humanState = HumanState.ZOMBIE;
        canChange = false;
        radius = 1.5f;

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


}
