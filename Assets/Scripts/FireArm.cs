using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : MonoBehaviour
{

    protected float damage;
    protected float shotCooldown;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawnPoint;    

    private void OnEnable()
    {
        PlayerInput.OnMouseButtonPressed += Fire;

    }
    private void OnDisable()
    {
        PlayerInput.OnMouseButtonPressed -= Fire;
    }


    protected virtual void Fire(Vector2 mousePos) {
        Vector2 direction = mousePos - (Vector2)transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;        

        Instantiate(projectile, projectileSpawnPoint.position, Quaternion.identity);
    }

    


}
