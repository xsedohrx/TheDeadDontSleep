using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] float projectileSpeed = 5.0f;
    public float force;
    private Rigidbody rigidbody;
    public static Action<float> OnTargetHit;
    public float projectileDamage = 4;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Start()
    {
        transform.parent = null;

        rigidbody.useGravity = true;
        rigidbody.isKinematic = false;

        rigidbody.AddForce(transform.forward * 5000f);

        Destroy(gameObject, 3f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Zombie" || collision.gameObject.tag == "Human" || collision.gameObject.tag == "Soldier" )
        {
            NPC npc = collision.gameObject.GetComponent<NPC>();
            npc?.TakeDamage(projectileDamage);
            Destroy(gameObject, 0.01f);
        }
    }

}
