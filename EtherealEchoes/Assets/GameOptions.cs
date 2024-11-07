using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOptions : MonoBehaviour
{
    //Фуллскрин
    public void FullScreen()
    {
        Screen.fullScreen = !Screen.fullScreen;
    }
}
