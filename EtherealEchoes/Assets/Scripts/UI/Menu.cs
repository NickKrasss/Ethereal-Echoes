using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;

    public string sceneToLoad;

    private void MakeButtonSound()
    {
        AudioManager.Instance.PlayAudio(buttonSound, SoundType.SFX, 1, 0f, 0.1f);
    }

    public void StartGame()
    {
        StopAllCoroutines();
        StartCoroutine(StartGameIE());
    }

    public void ExitGame()
    {
        StopAllCoroutines();
        StartCoroutine(ExitGameIE());
    }

    private IEnumerator StartGameIE()
    {
        MakeButtonSound();
        TransitionOverlayController.Instance.FadeIn(0.5f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(1);
    }

    private IEnumerator ExitGameIE()
    {
        MakeButtonSound();
        yield return new WaitForSeconds(0.2f);
        Application.Quit();
    }
}
