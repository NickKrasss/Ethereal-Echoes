using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSettingsInGame : MonoBehaviour
{
    //���� �������� ���� � �����
    public GameObject settingsMenu;
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    //����������� � ���� �����
    public void BackToPauseMenu()
    {
        settingsMenu.SetActive(false);
        gameObject.GetComponent<Pause>().pauseScreen.SetActive(true);
    }

    public void FullScreenToggle()
    {
        
    }

}
