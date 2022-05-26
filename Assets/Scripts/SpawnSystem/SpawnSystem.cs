using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    protected ObjectPooler objectPooler;
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State { get { return state; }}    
    [SerializeField] protected GameObject npcToSpawn;

    [SerializeField] private int unitsToSpawn = 10;
    [SerializeField] private float spawnDelay = .5f;

    protected virtual void Awake(){ objectPooler = ObjectPooler.Instance; }
    protected virtual void Start()
    {
        StartCoroutine(SpawnObject());
    }

    protected virtual IEnumerator SpawnObject()
    {
        if (unitsToSpawn > 0)
        {
            SpawnUnit();
            unitsToSpawn--;
            yield return new WaitForSeconds(spawnDelay);

        }

    }

    protected virtual void SpawnUnit(){}

}
