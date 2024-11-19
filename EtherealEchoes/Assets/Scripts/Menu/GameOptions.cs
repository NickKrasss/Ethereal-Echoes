using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class GameOptions : MonoBehaviour
{
    public TMP_Dropdown dropdown;
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
            Screen.SetResolution(1920, 1080,true);
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
}
