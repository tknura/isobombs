using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public UnityEvent OnGameOver;

    private bool isPaused;

    private void Awake() {
        if(!instance) {
            instance = this;
        }
    }

    private void Start() {
        Time.timeScale = 1;
        isPaused = false;
    }

    public void PauseGame() {
        if(!isPaused) {
            Time.timeScale = 0;
            isPaused = true;
        }
    }
    
    public void ResumeGame() {
        if(isPaused) {
            Time.timeScale = 1;
            isPaused = false;
        }
    }

    public void GameOver() {
        PauseGame();
        OnGameOver.Invoke();
    }
}
