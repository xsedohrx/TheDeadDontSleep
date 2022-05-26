using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : MonoBehaviour
{

    protected float damage;
    public float shotCooldown;
    public bool canFire = true;
    [SerializeField] GameObject projectile;
    [SerializeField] Transform projectileSpawnPoint;    

    private void OnEnable(){ PlayerInput.OnMouseButtonPressed += Fire; }
    private void OnDisable(){ PlayerInput.OnMouseButtonPressed -= Fire; }


    public virtual void Fire() {

        var projectileObj = Instantiate(projectile, projectileSpawnPoint);
        projectileObj.transform.localPosition = Vector3.zero;

    }

}
