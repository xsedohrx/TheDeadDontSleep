using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject player;
    List<GameObject> survivorsLeft;
    List<GameObject> soldiersLeft;
    List<GameObject> zombiesLeft;
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
        soldiersLeft = new List<GameObject>();  
        zombiesLeft = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        

    }

    private void Start()
    {
        fmodPlayer.StopAllMusic();
        FMODUnity.RuntimeManager.PlayOneShot("event:/TheDeadDontSleep/Music/HorrorTheme", gameObject.transform.position);
    }

    void RemoveFromList(List<GameObject> listToRemoveFrom, GameObject objectToRemove) {
        listToRemoveFrom.Remove(objectToRemove);
    }

    void AddToList(List<GameObject> listToAddTo, GameObject objectToAdd) {         
        listToAddTo.Add(objectToAdd);
    }




}
