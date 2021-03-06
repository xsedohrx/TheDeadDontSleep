using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierSpawner : SpawnSystem
{
    public enum SoldierType{ SOLDIER }
    public SoldierType soldierType;

    protected override void Awake()
    {
        base.Awake();
        soldierType = SoldierType.SOLDIER;
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
        GameObject soldierGO = objectPooler.SpawnFromPool(soldierType.ToString(), spawnLocations[Random.Range(0, spawnLocations.Length)].position, Quaternion.identity);
        soldierGO.GetComponent<Soldier>()?.ResetStats();

    }
}
