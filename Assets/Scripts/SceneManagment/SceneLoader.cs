using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneLoader : MonoBehaviour
{

    public void Awake()
    {
        DontDestroyOnLoad(this);
    }

    public void ChangeScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }

    public void ShowScoreBoard(GameObject scoreBoard) {
        scoreBoard.SetActive(!scoreBoard.activeSelf);
        Debug.Log("Scoreboard Active: " + scoreBoard.activeSelf);
    }
    

    public void ExitGame() {
        Application.Quit();
    }

}
