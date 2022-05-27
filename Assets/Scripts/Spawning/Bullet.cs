using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : AutoDestroyPoolableObject
{
    [HideInInspector]
    public Rigidbody rigidBody;
    public Vector3 speed = new Vector3(200, 0);

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    public override void OnEnable()
    {
        base.OnEnable();
        rigidBody.velocity = speed;
    }

    public override void OnDisable() {

        base.OnDisable();
        rigidBody.velocity = Vector3.zero;
    }

}
