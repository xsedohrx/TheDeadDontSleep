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

    FmodPlayer fmodPlayer;
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
        fmodPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<FmodPlayer>();
        survivorsLeft = new List<GameObject>();
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
        //Debug.Log("Zombies Left: " + zombiesLeft.Count);
        zombieCount--;
    }

    public void AddToZombies(GameObject objectToAdd) {         
        zombiesLeft.Add(objectToAdd);
        //Debug.Log("Zombies Left: " + zombiesLeft.Count);
        zombieCount++;
    }

    public void RemoveFromSoldiers(GameObject objectToRemove) {
        soldiersLeft.Remove(objectToRemove);
        //Debug.Log("Soldiers LEft: " + soldiersLeft.Count);
        soldierCount--;
    }

    public void AddToSoldiers(GameObject objectToAdd) {
        soldiersLeft.Add(objectToAdd);
        //Debug.Log("Soldiers LEft: "+soldiersLeft.Count);
        soldierCount++;
    }


    public int score = 0;
    public int wave = 1;
    float startTime;
    float waveStartTime;

    public int zombieCount = 0;
    public int soldierCount = 0;

    private void Start()
    {
        startTime = Time.time;
    }

    private void Update()
    {
        score++;

        if ( (zombiesLeft.Count < 10 && Time.time - waveStartTime > 10) )
        {
            score += 1000;
            waveStartTime = Time.time;
            wave++;
            //next wave..
            zombieSpawner.numberToSpawn = 40 * wave;
        }
    }

}
