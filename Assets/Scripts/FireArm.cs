using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireArm : MonoBehaviour
{

    protected float damage;
    [field: SerializeField] private float ammo = 30f;
    public float shotCooldown;
    public bool canFire = true;
    private ObjectPool bulletPool;
    [SerializeField] Bullet projectile;
    [SerializeField] Transform projectileSpawnPoint;

    public float Ammo { get { return ammo; } private set { } }
    private void Awake()
    {
        bulletPool = ObjectPool.CreateInstance(projectile, 30);
    }

    private void Start()
    {
    }

    public void Fire()
    {
        if(canFire && ammo > 0)
        {
            ammo--;
            PoolableObject instance = bulletPool.GetObject();
            instance.transform.SetParent(projectileSpawnPoint);
            instance.transform.localPosition = Vector3.zero;
            instance.transform.localRotation = Quaternion.identity;
            instance.gameObject.SetActive(true);
            instance.transform.SetParent(bulletPool.poolObject.transform);
            canFire = false;
            StartCoroutine(PauseBetweenFire());
        }
    }

    public IEnumerator PauseBetweenFire()
    {
        yield return new WaitForSeconds(shotCooldown);
        canFire = true;
    }
}
