using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    GameObject player;
    List<GameObject> soldiersLeft;
    List<GameObject> zombiesLeft;

    [SerializeField] Spawner zombieSpawner;
    [SerializeField] Spawner soldierSpawner;

    //private void OnEnable()
    //{
    //    NPC.OnDeath += RemoveFromList;
    //}

    //private void OnDisable()
    //{
    //    NPC.OnDeath -= RemoveFromList;
    //}

    private void Awake()
    {
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }
        else if (instance != this)
        {
            Debug.LogWarning(this.GetType().Name + " - Destroying secondary instance. Instance Id : " + gameObject.GetInstanceID());

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GlobalManager.
            DestroyImmediate(gameObject);

            return;
        }

        soldiersLeft = new List<GameObject>();  
        zombiesLeft = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");   
    }

    public void RemoveFromZombies(GameObject objectToRemove) {
        zombiesLeft.Remove(objectToRemove);
        Debug.Log("Zombies Left: " + zombiesLeft.Count);
    }

    public void AddToZombies(GameObject objectToAdd) {         
        zombiesLeft.Add(objectToAdd);
        Debug.Log("Zombies Left: " + zombiesLeft.Count);
    }

    public void RemoveFromSoldiers(GameObject objectToRemove) {
        soldiersLeft.Remove(objectToRemove);
        Debug.Log("Soldiers LEft: " + soldiersLeft.Count);
    }

    public void AddToSoldiers(GameObject objectToAdd) {
        soldiersLeft.Add(objectToAdd);
        Debug.Log("Soldiers LEft: "+soldiersLeft.Count);
    }


    int wave = 1;
    float startTime;
    float waveStartTime;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        
        if(zombiesLeft.Count < 10 && Time.time - waveStartTime > 10)
        {
            waveStartTime = Time.time;
            wave++;
            //next wave..
            zombieSpawner.numberToSpawn = 40 * wave;
        }
    }

}
