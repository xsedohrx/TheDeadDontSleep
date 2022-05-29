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

    protected override IEnumerator SpawnObject()
    {
        return base.SpawnObject();
    }


    protected override void SpawnUnit()
    {
        base.SpawnUnit();
        PoolableObject poolableObject = objectPooler.GetObject();
        poolableObject.gameObject.SetActive(true);

        poolableObject.gameObject.transform.position = spawnLocations[Random.Range(0, spawnLocations.Length)].position;
        poolableObject.gameObject.transform.rotation = Quaternion.identity;

        (poolableObject as Zombie)?.ResetStats();
    }
}
