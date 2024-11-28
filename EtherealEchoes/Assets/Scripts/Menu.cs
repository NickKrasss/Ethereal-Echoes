using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public string sceneToLoad;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject audioOptions;
    [SerializeField] private GameObject gameOptions;
    [SerializeField] private GameObject postEffectOptions;
    //Загрузка сцены настроек
    public void LoadOptionsMenu(string s)
    {
        mainMenu.SetActive(s[0] == '1');
        optionsMenu.SetActive(s[1] == '1');
        audioOptions.SetActive(s[2] == '1');
        gameOptions.SetActive(s[3] == '1');
        postEffectOptions.SetActive(s[4] == '1');
    }


    //Фуллскрин
    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
    //Настройка разрешений
    public void ChangeRelosution()
    {
        if (dropdown.value == 0)
        {
            Screen.SetResolution(1920, 1080, true);
        }
        else if (dropdown.value == 1)
        {
            Screen.SetResolution(1366, 768, true);
        }
        else if (dropdown.value == 2)
        {
            Screen.SetResolution(1280, 800, true);
        }
        else if (dropdown.value == 3)
        {
            Screen.SetResolution(1440, 900, true);
        }
        else if (dropdown.value == 4)
        {
            Screen.SetResolution(1280, 1024, true);
        }
        else if (dropdown.value == 5)
        {
            Screen.SetResolution(1600, 900, true);
        }
        else if (dropdown.value == 6)
        {
            Screen.SetResolution(2560, 1440, true);
        }
        else if (dropdown.value == 7)
        {
            Screen.SetResolution(3840, 2160, true);
        }
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
    void Start()
    {

    }
    void LoadGameScene()
    {
        // Загружаем сцену
        SceneManager.LoadScene(sceneToLoad);
    }
    void Update()
    {

    }
    //Выход из игры
    public void ExitGame()
    {
        Application.Quit();
    }
}
