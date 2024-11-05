using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
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
