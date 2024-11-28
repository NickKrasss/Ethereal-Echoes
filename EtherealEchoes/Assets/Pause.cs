using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    private bool isPaused = false;
    [SerializeField] private GameObject pauseButton;
    void Start()
    {
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) ResumeGame();
            else PauseGame();
        }
    }
    public void PauseGame()
    {
        pauseButton.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
        pauseButton.SetActive(false);
    }
    public void LoadMainMenu()
    {
        ResumeGame();
        SceneManager.LoadScene(0);
    }
}
