using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FmodPlayer : MonoBehaviour
{

    FMOD.Studio.Bus MasterBus;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        MasterBus = FMODUnity.RuntimeManager.GetBus("Bus:/");
    }

    private void Start()
    {
        StopAllMusic();

        PlaySound("event:/TheDeadDontSleep/Music/MusicBox/MainMenuTheme");
    }

    public void PlaySound(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path, GetComponent<Transform>().position);
    }

    public void StopAllMusic()
    {
        MasterBus.stopAllEvents(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
