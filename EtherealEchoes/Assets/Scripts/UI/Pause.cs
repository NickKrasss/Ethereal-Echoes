using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Pause : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseScreen;
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
        pauseScreen.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        AudioListener.pause = true;
    }
    public void ResumeGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
        pauseScreen.SetActive(false);
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        isPaused = false;
        AudioListener.pause = false;
        Destroy(AudioManager.Instance.gameObject);
        TransitionOverlayController.Instance.FadeIn(0.15f, 0f, () =>
        {
            Destroy(G.Instance.gameObject); 
            SceneManager.LoadScene(0);
        });
    }
}
