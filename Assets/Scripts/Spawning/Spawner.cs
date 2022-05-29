using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Spawner : MonoBehaviour
{

    [SerializeField] private int numberToSpawn = 10;
    [SerializeField] private float spawnDelay = 1f;
    [SerializeField] private List<NPC> unitPrefabs = new List<NPC> ();
    public SpawnMethod spawnMethod = SpawnMethod.ROUNDROBIN;
    private NavMeshTriangulation triangulation;

    private Dictionary<int, ObjectPool> unitObjectPool = new Dictionary<int, ObjectPool> ();
    private void Awake()
    {
        for (int i = 0; i < unitPrefabs.Count; i++)
        {
            unitObjectPool.Add(i, ObjectPool.CreateInstance(unitPrefabs[i], numberToSpawn));
        }
    }

    private void Start()
    {
        triangulation = NavMesh.CalculateTriangulation();
        StartCoroutine(SpawnUnits());
    }

    private IEnumerator SpawnUnits() {
        WaitForSeconds wait = new WaitForSeconds(spawnDelay);
        
        int spawnedUnits = 0;
        
        while (spawnedUnits < numberToSpawn)
        {
            if (spawnMethod == SpawnMethod.ROUNDROBIN)
            {
                SpawnRoundRobinUnit(spawnedUnits);
            }
            else if (spawnMethod == SpawnMethod.RANDOM) {
                SpawnRandomUnit();
            }
            spawnedUnits++;
            
            yield return wait;
        }
    }

    private void SpawnRoundRobinUnit(int spawnedUnits) { 
        int spawnIndex = spawnedUnits % unitPrefabs.Count;

        DoSpawnUnit(spawnIndex);
    }

    private void SpawnRandomUnit() { 
        DoSpawnUnit(UnityEngine.Random.Range(0, unitPrefabs.Count));
    }

    private void DoSpawnUnit(int spawnIndex)
    {
        PoolableObject poolableObject = unitObjectPool[spawnIndex].GetObject();
        
        NPC unit = poolableObject.GetComponent<NPC>();

        do
        {
            int vertexIndex = UnityEngine.Random.Range(0, triangulation.vertices.Length);
            NavMeshHit hit;
            if (NavMesh.SamplePosition(triangulation.vertices[vertexIndex], out hit, 2f, -1))
            {
                NavMeshAgent agent = unit.GetComponent<NavMeshAgent>();
                agent.Warp(hit.position);
                //Enable unit
                agent.enabled = true;
            }
            //repeat if this locations cant get to the central spawn point.. ie out of bounds
        } while (unit.CalculatePathLength(transform.position) == float.PositiveInfinity);

        poolableObject.gameObject.SetActive(true);
    }

    public enum SpawnMethod { 
        ROUNDROBIN,
        RANDOM
    }

}
