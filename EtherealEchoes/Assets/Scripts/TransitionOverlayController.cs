using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TransitionOverlayController : MonoBehaviour
{
    public static TransitionOverlayController Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        StopAllCoroutines();
        background.color = new Color(0, 0, 0, 0);
        text.color = new Color(1, 1, 1, 0);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    bool fadedIn;

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if (fadedIn)
        {
            StopAllCoroutines();
            FadeOut(0.5f, 0.5f);
        }
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    [SerializeField] Image background;
    [SerializeField] TMP_Text text;

    public void FadeIn(float duration, float delay = 0f, Action action = null)
    {
        StartCoroutine(fadeAction(new Color(0, 0, 0, 1), new Color(1, 1, 1, 1), duration, delay, action));
        fadedIn = true; 
    }

    public void FadeOut(float duration, float delay = 0f, Action action = null)
    {
        StartCoroutine(fadeAction(new Color(0, 0, 0, 0), new Color(1, 1, 1, 0), duration, delay, action));
        fadedIn = false;
    }

    IEnumerator fadeAction(Color desiredColor, Color desiredTextColor, float time, float delay = 0f, Action action = null)
    {
        yield return new WaitForSeconds(delay);

        Color startColor = background.color;
        Color startTextColor = text.color;

        float elapsedTime = 0f;
        while (elapsedTime < time)
        {
            elapsedTime += Time.unscaledDeltaTime;
            text.color = Color.Lerp(startTextColor, desiredTextColor, elapsedTime / time);
            background.color = Color.Lerp(startColor, desiredColor, elapsedTime / time);
            yield return null;
        }
        text.color = desiredTextColor;
        background.color = desiredColor;

        action?.Invoke();
    }
}
