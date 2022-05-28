using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : MonoBehaviour
{

    protected float damage;
    public float shotCooldown;
    public bool canFire = true;
    private ObjectPool bulletPool;
    [SerializeField] Bullet projectile;
    [SerializeField] Transform projectileSpawnPoint;    

    //private void OnEnable(){ PlayerInput.OnMouseButtonPressed += Fire; }
    //private void OnDisable(){ PlayerInput.OnMouseButtonPressed -= Fire; }


    private void Awake()
    {
        bulletPool = ObjectPool.CreateInstance(projectile, 30);
    }

    private void Start()
    {
        StartCoroutine(Fire());
    }

    public IEnumerator Fire()
    {
        WaitForSeconds wait = new WaitForSeconds(1.0f / shotCooldown);

        while (canFire)
        {
            PoolableObject instance = bulletPool.GetObject();
            if (instance != null)
            {
                instance.transform.SetParent(transform, false);
                instance.transform.localPosition = Vector3.zero;
            }
            canFire = false;
            yield return wait;
            canFire = true;
        }
    }

}
