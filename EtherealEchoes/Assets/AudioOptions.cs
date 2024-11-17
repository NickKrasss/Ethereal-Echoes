using System.Collections;
using System.Collections.Generic;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.UI;

public class AudioOptions : MonoBehaviour
{
    public Slider slider;
    public Toggle toggle;
    void Start()
    {
        if (PlayerPrefs.HasKey("Volume"))
        {
            slider.value = PlayerPrefs.GetFloat("Volume");
        }
        else
        {
            PlayerPrefs.SetFloat("Volume",0.5f);
            slider.value = 0.5f;
        }
        if (toggle)
        {
            PlayerPrefs.SetInt("Flag", 1);
        }
        else
        {
            PlayerPrefs.SetInt("Flag", 0);
        }
    }
    //Изменение громкости 
    public void Change_Volume(float x)
    {
        PlayerPrefs.SetFloat("Volume", x);
    }
    public void Change_Flag(bool x)
    {
        if (x)
        {
            PlayerPrefs.SetInt("Flag",1);
        }
        else
        {
            PlayerPrefs.SetInt("Flag",0);
        }
    }
}
