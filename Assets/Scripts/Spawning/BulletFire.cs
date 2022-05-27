using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletFire : MonoBehaviour
{

    public Bullet bulletPrefab;
    public int rateOfFire = 5;
    private ObjectPool bulletPool;

    private void Awake() {
        bulletPool = ObjectPool.CreateInstance(bulletPrefab, 50) ;
    }

    private void Start()
    {
        StartCoroutine(Fire());
    }

    private IEnumerator Fire() {
        WaitForSeconds wait = new WaitForSeconds(1.0f / rateOfFire);

        while (true)
        {
            PoolableObject instance = bulletPool.GetObject();
            if (instance != null) {
                instance.transform.SetParent(transform, false);
                instance.transform.localPosition = Vector3.zero;
            }
            yield return wait;
        }
    }
}