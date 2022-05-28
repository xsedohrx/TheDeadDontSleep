using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{

    private ObjectPool bulletPool;
    private Bullet bullet;
    private void Awake()
    {
        bulletPool = ObjectPool.CreateInstance(bullet, 100);
    }

    void GetObject() {
        bulletPool.GetObject();
    }
}
