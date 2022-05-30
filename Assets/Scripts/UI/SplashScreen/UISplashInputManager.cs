using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class UISplashInputManager : MonoBehaviour
{
    private VisualElement root;
    private Button playButton;
    private Button exitButton;
    private SceneLoader loader;


    private void PlayGame()
    {
        GameObject.Find("SceneManager").GetComponent<SceneLoader>().ChangeScene("Prototype");
    }

    private void ExitGame()
    {
        GameObject.Find("SceneManager").GetComponent<SceneLoader>().ExitGame();
    }

  
    private void Awake()
    {
        root = GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<Button>("play");
        exitButton = root.Q<Button>("exit");

    }
    void Start()
    {
        playButton.clicked += PlayGame;
        exitButton.clicked += ExitGame;
    }


    void Update()
    {

    }
}
