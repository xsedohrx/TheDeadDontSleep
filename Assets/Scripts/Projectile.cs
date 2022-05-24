using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] float projectileSpeed = 5.0f;
    public float force;
    Rigidbody rigidbody;
    public static Action<float> OnTargetHit;
    public float projectileDamage = 2;
    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
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
        rigidbody.velocity = new Vector3(direction.x, 0, direction.y).normalized * force;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg * force;
        transform.rotation = Quaternion.Euler(0, 0, rot - 90);
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Unit")
        {
            OnTargetHit?.Invoke(projectileDamage);
            
        }
    }

}
