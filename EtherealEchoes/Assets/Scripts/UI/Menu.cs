using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    private AudioSource source;
    [SerializeField] private AudioClip buttonSound;

    public TMP_Dropdown dropdown;
    public string sceneToLoad;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject audioOptions;
    [SerializeField] private GameObject gameOptions;
    [SerializeField] private GameObject postEffectOptions;
    [SerializeField] private GameObject toggle;
    //Загрузка сцены настроек
    public void LoadOptionsMenu(string s)
    {
        source = GetComponent<AudioSource>();
        mainMenu.SetActive(s[0] == '1');
        optionsMenu.SetActive(s[1] == '1');
        audioOptions.SetActive(s[2] == '1');
        gameOptions.SetActive(s[3] == '1');
        postEffectOptions.SetActive(s[4] == '1');
        MakeButtonSound();
    }

    private void Awake()
    {
        PlayerPrefs.SetInt("GodMode", 0);
    }

    public void GetGodModeValue()
    {
        MakeButtonSound();
        if (toggle.GetComponent<Toggle>().isOn)
        {
            PlayerPrefs.SetInt("GodMode", 1);
        }
        else
        {
            PlayerPrefs.SetInt("GodMode", 0);
        }

    }

    private void MakeButtonSound()
    {
        AudioManager.Instance.PlayAudio(buttonSound, SoundType.SFX, 1, 0f, 0.1f);
    }

    //Фуллскрин
    public void FullScreen()
    {
        MakeButtonSound();
        Screen.fullScreen = !Screen.fullScreen;
    }
    //Настройка разрешений
    public void ChangeRelosution()
    {

        if (dropdown.value == 0)
        {
            Screen.SetResolution(1920, 1080, Screen.fullScreen);
        }
        else if (dropdown.value == 1)
        {
            Screen.SetResolution(1366, 768, Screen.fullScreen);
        }
        else if (dropdown.value == 2)
        {
            Screen.SetResolution(1280, 800, Screen.fullScreen);
        }
        else if (dropdown.value == 3)
        {
            Screen.SetResolution(1440, 900, Screen.fullScreen);
        }
        else if (dropdown.value == 4)
        {
            Screen.SetResolution(1280, 1024, Screen.fullScreen);
        }
        else if (dropdown.value == 5)
        {
            Screen.SetResolution(1600, 900, Screen.fullScreen);
        }
        else if (dropdown.value == 6)
        {
            Screen.SetResolution(2560, 1440, Screen.fullScreen);
        }
        else if (dropdown.value == 7)
        {
            Screen.SetResolution(3840, 2160, Screen.fullScreen);
        }
    }

    public void StartGame()
    {
        StartCoroutine(StartGameIE());
    }

    public void ExitGame()
    {
        StartCoroutine(ExitGameIE());
    }

    private IEnumerator StartGameIE()
    {
        MakeButtonSound();
        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(1);
    }

    //Выход из игры
    private IEnumerator ExitGameIE()
    {
        MakeButtonSound();
        yield return new WaitForSeconds(0.2f);
        Application.Quit();
    }

    // Включение/отключение всех звуков в игре
    public void ToggleSound()
    {
        MakeButtonSound();
        AudioListener.pause = !AudioListener.pause;
    }

}
