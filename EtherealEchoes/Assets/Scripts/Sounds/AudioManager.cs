using UnityEngine;
using UnityEngine.UI; // Для работы с UI и слайдерами
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Слайдер громкости музыки")]
    [SerializeField] private Slider musicVolumeSlider; // Слайдер громкости музыки
    [Header("Слайдер громкости звуков")]
    [SerializeField] private Slider soundVolumeSlider; // Слайдер громкости звуков

    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

 
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
    }

    private void Update()
    {
        CheckAllSourcses();
    }

    // Метод для проигрывания аудио
    public AudioSource PlayAudio(AudioClip clip, SoundType type, float volumeMultiplier = 1f, float volumeRandomOffset = 0f, float randomPitchOffset = 0f)
    {
        AudioSource source = CreateAudioSource();
        if (!clip)
            return source;
        UpdateVolume(source, type, volumeMultiplier);
        var rvol = Random.Range(-volumeRandomOffset / 2, volumeRandomOffset / 2);
        if (source.volume > 0 && source.volume + rvol > 0) source.volume += rvol;
        source.pitch = 1 + Random.Range(-randomPitchOffset/2, randomPitchOffset/2);

        source.PlayOneShot(clip);
        return source;
    }

    public void UpdateVolume(AudioSource source, SoundType type, float volumeMultiplier = 1f)
    {
        if (type == SoundType.SFX)
            source.volume = PlayerPrefs.GetFloat("SoundsVolume", 1.0f) * volumeMultiplier;
        else if (type == SoundType.Music)
            source.volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f) * volumeMultiplier;
    }

    public void SmoothVolumeChange(AudioSource source, float volume, float speed)
    { 
        source.volume = Mathf.Lerp(source.volume, volume, speed*Time.deltaTime);
    }

    private AudioSource CreateAudioSource()
    {
        GameObject auSourceObj = new GameObject($"AudioSource_{audioSources.Count}");
        //auSourceObj.transform.SetParent(transform);
        AudioSource auSource = auSourceObj.AddComponent<AudioSource>();
        audioSources.Add(auSource);
        return auSource;
    }

    private void CheckAllSourcses()
    {
        foreach (AudioSource source in audioSources)
        {
            if (!source.isPlaying)
            {
                Destroy(source.gameObject);
                audioSources.Remove(source);
                break;
            }
        }
    }

    // Обработчик изменения громкости музыки
    private void OnMusicVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat("MusicVolume", volume); // Сохраняем значение в PlayerPrefs
        PlayerPrefs.Save(); // Сохраняем изменения
    }

    // Обработчик изменения громкости звуков
    private void OnSoundVolumeChanged(float volume)
    {
        PlayerPrefs.SetFloat("SoundsVolume", volume); // Сохраняем значение в PlayerPrefs
        PlayerPrefs.Save(); // Сохраняем изменения
    }
}

public enum SoundType
{ 
    SFX, Music
}
