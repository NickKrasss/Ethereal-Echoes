using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SFXManager : MonoBehaviour
{
    [Header("Саунд-эффекты для кнопок")]
    public List<AudioClip> buttonSounds; 
    [Header("Аудиоисточник для звуков")]
    public AudioSource sfxSource; 
    [Header("Слайдер для регулировки громкости")]
    public Slider sfxVolumeSlider; 

    private float sfxVolume = 1.0f;
    public float pitchMin = 0.9f; 
    public float pitchMax = 1.1f; 

    private void Start()
    {
        if (sfxSource == null)
        {
            Debug.LogError("AudioSource для SFX не назначен!");
            return;
        }

        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);
        sfxSource.volume = sfxVolume;
        sfxVolumeSlider.value = sfxVolume;

        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        Button[] buttons = FindObjectsOfType<Button>();  
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(() => PlayRandomButtonSFX()); 
        }
    }

   
    private void PlayRandomButtonSFX()
    {
        if (buttonSounds.Count > 0)
        {
            int randomIndex = Random.Range(0, buttonSounds.Count);  
            AudioClip randomClip = buttonSounds[randomIndex];  

            
            float randomPitch = Random.Range(pitchMin, pitchMax); 
            sfxSource.pitch = randomPitch;

            sfxSource.PlayOneShot(randomClip); 
        }
    }

    
    public void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }
}
