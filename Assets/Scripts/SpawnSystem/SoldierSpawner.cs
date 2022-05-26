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

    protected override void SpawnUnit()
    {
        base.SpawnUnit();
        //Debug.Log(soldierType.ToString());
        objectPooler.SpawnFromPool(soldierType.ToString(), transform.position, Quaternion.identity);
    }
}
