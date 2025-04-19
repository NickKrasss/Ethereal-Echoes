using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [SerializeField] private AudioClip buttonSound;

    #region Resolution
    public TMP_Dropdown screenResolutions_dropdown;
    int currentResolutionIndex = 0;
    List<string> resolutionOptions = new List<string>();
    #endregion

    public string sceneToLoad;
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject optionsMenu;
    [SerializeField] private GameObject audioOptions;
    [SerializeField] private GameObject gameOptions;
    [SerializeField] private GameObject postEffectOptions;
    [SerializeField] private GameObject toggle;

    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Slider cameraShakeSlider;
    [SerializeField] private Toggle godModeToggle;


    //Загрузка сцены настроек
    public void LoadOptionsMenu(string s)
    {
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
        if (godModeToggle != null)
            godModeToggle.isOn = false;


        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        if (soundVolumeSlider != null)
        {
            soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundsVolume", 1.0f);
            soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        }

        if (cameraShakeSlider != null)
        {
            cameraShakeSlider.value = PlayerPrefs.GetFloat("CameraShakeForce", 1.0f);
            cameraShakeSlider.onValueChanged.AddListener(OnCameraShakeForceChanged);
        }

        currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);
    }

    private void Start()
    {
        #region Adding available resolution options to the dropdown
        screenResolutions_dropdown.ClearOptions();

        Resolution[] resolutions = Screen.resolutions;
        resolutionOptions = new List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;

            resolutionOptions.Add(option);
        }

        resolutionOptions.Reverse(); // start from highest resolution
        resolutionOptions = resolutionOptions.Distinct().ToList(); // remove duplicates

        screenResolutions_dropdown.AddOptions(resolutionOptions);
        screenResolutions_dropdown.value = currentResolutionIndex;

        screenResolutions_dropdown.RefreshShownValue();

        // Set the current resolution
        ChangeResolution(currentResolutionIndex);
        #endregion
    }
    private void OnMusicVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    private void OnSoundVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat("SoundsVolume", volume);
        PlayerPrefs.Save();
    }

    private void OnCameraShakeForceChanged(float volume)
    {
        PlayerPrefs.SetFloat("CameraShakeForce", volume);
        PlayerPrefs.Save();
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
    public void ChangeResolution(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();

        int width = int.Parse(resolutionOptions[index].Split(" x ")[0]);
        int height = int.Parse(resolutionOptions[index].Split(" x ")[1]);

        Screen.SetResolution(width, height, Screen.fullScreen);
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

}
