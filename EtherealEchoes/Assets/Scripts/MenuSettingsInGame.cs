using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettingsInGame : MonoBehaviour
{
    //Меню настроек игры в паузе
    public GameObject settingsMenu;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //Возвращение в меню паузы
    public void BackToPauseMenu()
    {
        settingsMenu.SetActive(false);
        gameObject.GetComponent<Pause>().pauseScreen.SetActive(true);
    }

    public void FullScreenToggle()
    {
        
    }

}
