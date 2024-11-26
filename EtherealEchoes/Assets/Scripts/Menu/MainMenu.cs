using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string menuMusic;
    private void Start()
    {
        // Проверка имени объекта
        if (gameObject.name.StartsWith("Main"))
        {
            // Попробуем проиграть музыку через AudioManager
            AudioManager.Instance.PlayMusic("MenuSoundtrack", 0.7f);
        }
        else
        {
            Debug.LogError("MainMenu: This object does not start with 'Main'. Music will not play.");
        }
        Update();
    }
    private void Update()
    {

    }
    //Запускает начальный уровень игры
    public void PlayGame()
    {
        SceneManager.LoadScene(1);
    }
    //
    public void ExitGame()
    {
        Application.Quit();
    }
}
