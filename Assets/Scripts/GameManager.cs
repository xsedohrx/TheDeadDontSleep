using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    GameObject player;
    FmodPlayer fmodPlayer;
    List<GameObject> survivorsLeft;
    List<GameObject> soldiersLeft;
    List<GameObject> zombiesLeft;

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
        survivorsLeft = new List<GameObject>();
        soldiersLeft = new List<GameObject>();  
        zombiesLeft = new List<GameObject>();
        player = GameObject.FindGameObjectWithTag("Player");
        fmodPlayer = GameObject.FindGameObjectWithTag("AudioManager").GetComponent<FmodPlayer>();

    }

    private void Start()
    {
        fmodPlayer.StopAllMusic();
        fmodPlayer.PlaySound("event:/TheDeadDontSleep/Music/HorrorTheme");
    }

    void RemoveFromList(List<GameObject> listToRemoveFrom, GameObject objectToRemove) {
        listToRemoveFrom.Remove(objectToRemove);
    }

    void AddToList(List<GameObject> listToAddTo, GameObject objectToAdd) {         
        listToAddTo.Add(objectToAdd);
    }




}
