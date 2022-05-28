using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Bullet : AutoDestroyPoolableObject
{
    [HideInInspector]
    public Rigidbody rigidBody;
    public float speed = 5000f;
    public float projectileDamage = 4;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();
        rigidBody.isKinematic = false;

        rigidBody.AddForce(transform.forward * speed);
    }

    protected override void OnDisable() 
    {
        rigidBody.velocity = Vector3.zero;
        base.OnDisable();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Zombie" || collision.gameObject.tag == "Human" || collision.gameObject.tag == "Soldier")
        {
            NPC npc = collision.gameObject.GetComponent<NPC>();
            npc?.TakeDamage(projectileDamage);
            gameObject.SetActive(false);
        }
        else
        {
            //dont allow bounce off other objects
            gameObject.SetActive(false);
        }
    }

}
