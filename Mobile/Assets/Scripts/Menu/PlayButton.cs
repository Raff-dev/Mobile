using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayButton : MonoBehaviour {

    [SerializeField]
    private string gameSceneName = "orbital";

    public void startGame() {
        SceneManager.LoadScene(gameSceneName);
    }
}
