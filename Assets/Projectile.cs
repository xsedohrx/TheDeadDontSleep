using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] float projectileSpeed = 5.0f;
    public float force;
    Rigidbody2D rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Start()
    {
        SetRotation();
    }

    //TODO Fix bullet rotation on firing!
    private void SetRotation()
    {
        Vector3 direction = PlayerInput.mousePos - transform.position;
        Vector3 rotation = transform.position - PlayerInput.mousePos;
        rigidbody.velocity = new Vector2(direction.x, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg * force;
        transform.rotation = Quaternion.Euler(0, 0, rot - 90);
        
    }

}
