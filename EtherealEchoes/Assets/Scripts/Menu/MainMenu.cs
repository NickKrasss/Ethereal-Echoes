using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string menuMusic;
    private void Start()
    {
        // �������� ����� �������
        if (gameObject.name.StartsWith("Main"))
        {
            // ��������� ��������� ������ ����� AudioManager
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
    //��������� ��������� ������� ����
    public void PlayGame()
    {
        var audioSource = gameObject.GetComponent<AudioSource>();
        AudioManager.Instance.PlaySound("ButtonClick", audioSource);
        SceneManager.LoadScene(1);
    }
    public void ExitGame()
    {
        var audioSource = gameObject.GetComponent<AudioSource>();
        AudioManager.Instance.PlaySound("ButtonClick", audioSource);
        Application.Quit();
    }
}
