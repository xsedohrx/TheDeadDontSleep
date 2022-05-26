using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieSpawner : SpawnSystem
{
    public enum ZombieType{ZOMBIE}
    public ZombieType zombieType;

    protected override void Awake()
    {
        base.Awake();
        zombieType = ZombieType.ZOMBIE;
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void SpawnUnit()
    {
        base.SpawnUnit();
        objectPooler.SpawnFromPool(zombieType.ToString(), transform.position, Quaternion.identity);


    }
}
