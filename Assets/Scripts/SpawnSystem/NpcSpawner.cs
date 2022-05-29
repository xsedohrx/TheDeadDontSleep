using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcSpawner : SpawnSystem
{
    public enum SurvivorType { NPC }
    public SurvivorType survivorType;

    protected override void Awake() { 
        base.Awake();
        survivorType = SurvivorType.NPC;
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
    }
}
