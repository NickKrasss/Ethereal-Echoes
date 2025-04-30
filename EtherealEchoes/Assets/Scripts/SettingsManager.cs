using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public UnityEvent actionOnExit;
    public UnityEvent actionOnOpenSettings;
    [SerializeField] private AudioClip buttonSound;

    public Action onResolutionChanged;

    [Space(20)]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider soundVolumeSlider;
    [SerializeField] private Slider cameraShakeSlider;
    [SerializeField] private Toggle godModeToggle;
    [SerializeField] private Toggle fullScreenToggle;

    [Space(10)]

    #region Resolution
    public TMP_Dropdown screenResolutions_dropdown;
    int currentResolutionIndex = 0;
    List<string> resolutionOptions = new List<string>();
    #endregion

    public static SettingsManager instance { get; private set; }

    private void Awake()
    {
        instance = this;

        PlayerPrefs.SetInt("GodMode", 1);
        PlayerPrefs.Save();
        godModeToggle.isOn = false;

        musicVolumeSlider.value = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
        soundVolumeSlider.value = PlayerPrefs.GetFloat("SoundsVolume", 1.0f);
        cameraShakeSlider.value = PlayerPrefs.GetFloat("CameraShakeForce", 1.0f);
        fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        SwitchFullScreen(PlayerPrefs.GetInt("FullScreen", 1) == 1); // just in case
        currentResolutionIndex = PlayerPrefs.GetInt("ResolutionIndex", 0);

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

    public void OpenSettings()
    {
        actionOnOpenSettings?.Invoke();
    }

    public void SwitchFullScreen(bool value)
    {
        Screen.fullScreen = value;
        PlayerPrefs.SetInt("FullScreen", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void SwitchGodMode(bool value)
    {
        PlayerPrefs.SetInt("GodMode", value ? 1 : 0);
        PlayerPrefs.Save();
    }

    public void ChangeResolution(int index)
    {
        PlayerPrefs.SetInt("ResolutionIndex", index);
        PlayerPrefs.Save();

        int width = int.Parse(resolutionOptions[index].Split(" x ")[0]);
        int height = int.Parse(resolutionOptions[index].Split(" x ")[1]);

        Screen.SetResolution(width, height, Screen.fullScreen);
        onResolutionChanged?.Invoke();
    }

    public void OnMusicVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnSoundVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat("SoundsVolume", volume);
        PlayerPrefs.Save();
    }

    public void OnCameraShakeForceChanged(float volume)
    {
        PlayerPrefs.SetFloat("CameraShakeForce", volume);
        PlayerPrefs.Save();
    }

    public void MakeButtonSound()
    {
        AudioManager.Instance.PlayAudio(buttonSound, SoundType.SFX, 1, 0f, 0.1f);
    }

    public void Exit()
    {
        actionOnExit?.Invoke();
    }
}
