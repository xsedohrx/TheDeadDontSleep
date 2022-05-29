using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSystem : MonoBehaviour
{
    protected ObjectPool objectPooler;
    public enum SpawnState { SPAWNING, WAITING, COUNTING };
    private SpawnState state = SpawnState.WAITING;
    public SpawnState State { get { return state; }}    
    [SerializeField] protected PoolableObject npcToSpawn;

    [SerializeField] private int unitsToSpawn = 10;
    [SerializeField] private float spawnDelay = .5f;

    protected Transform[] spawnLocations;


    protected virtual void Awake(){ 
        objectPooler = ObjectPool.CreateInstance(npcToSpawn, 30);
        spawnLocations = GetComponentsInChildren<Transform>();
    }
    protected virtual void Start()
    {
        
    }

    protected void Update()
    {
        if (state == SpawnState.WAITING && unitsToSpawn > 0)
        {
            state = SpawnState.SPAWNING;
            SpawnUnit();
            unitsToSpawn--;
            StartCoroutine(SpawnObject());
        }
    }

    protected virtual IEnumerator SpawnObject()
    {
        yield return new WaitForSeconds(spawnDelay);
        state = SpawnState.WAITING;
    }

    protected virtual void SpawnUnit(){}

}
