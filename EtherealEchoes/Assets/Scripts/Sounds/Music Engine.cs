using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    [Header("Настройки")]
    public AudioSource soundtrackSource;
    [Header("Саундтреки")]
    public List<AudioClip> soundtracks;
    [Header("Длительность кроссфейда")]
    public float crossfadeDuration = 1.0f;

    [Header("Слайдер для звуков")]
    public AudioSource sfxSource;
    public Slider sfxVolumeSlider;

    [Header("Слайдер для музыки")]
    public Slider soundtrackVolumeSlider;

    private int currentTrackIndex = -1;
    private float soundtrackVolume = 1.0f;
    private float sfxVolume = 1.0f;
    [Header("Выключен ли звук")]
    public static bool isMuted = false;
    [Header("Кнопка мута")]
    public Button muteButton;

    private void Start()
    {
        soundtrackVolume = PlayerPrefs.GetFloat("SoundtrackVolume", 1.0f);
        sfxVolume = PlayerPrefs.GetFloat("SFXVolume", 1.0f);

        soundtrackVolumeSlider.value = soundtrackVolume;
        sfxVolumeSlider.value = sfxVolume;

        soundtrackSource.volume = soundtrackVolume;
        sfxSource.volume = sfxVolume;

        soundtrackVolumeSlider.onValueChanged.AddListener(SetSoundtrackVolume);
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);

        if (soundtracks.Count > 0)
        {
            StartCoroutine(PlayRandomSoundtrack());
        }
    }

    private void SetSoundtrackVolume(float value)
    {
        soundtrackVolume = value;
        soundtrackSource.volume = value;
        PlayerPrefs.SetFloat("SoundtrackVolume", value);
    }

    private void SetSFXVolume(float value)
    {
        sfxVolume = value;
        sfxSource.volume = value;
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    private IEnumerator PlayRandomSoundtrack()
    {
        while (true)
        {
            int newTrackIndex;
            do
            {
                newTrackIndex = Random.Range(0, soundtracks.Count);
            } while (newTrackIndex == currentTrackIndex);

            currentTrackIndex = newTrackIndex;
            AudioClip nextTrack = soundtracks[currentTrackIndex];

            yield return StartCoroutine(CrossfadeToTrack(nextTrack));

            yield return new WaitForSeconds(nextTrack.length - crossfadeDuration);
        }
    }

    private void ToggleMute()
    {
        isMuted = !isMuted;
        if (isMuted)
        {
            soundtrackSource.volume = 0;
        }
        else
        {
            soundtrackSource.volume = soundtrackVolume;
        }
    }

    private IEnumerator CrossfadeToTrack(AudioClip nextTrack)
    {
        float startVolume = soundtrackSource.volume;
        for (float t = 0; t < crossfadeDuration; t += Time.deltaTime)
        {
            soundtrackSource.volume = Mathf.Lerp(startVolume, 0, t / crossfadeDuration);
            yield return null;
        }

        soundtrackSource.clip = nextTrack;
        soundtrackSource.Play();

        for (float t = 0; t < crossfadeDuration; t += Time.deltaTime)
        {
            soundtrackSource.volume = Mathf.Lerp(0, soundtrackVolume, t / crossfadeDuration);
            yield return null;
        }

        soundtrackSource.volume = soundtrackVolume;
    }

    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip);
    }
}


public class ExampleSFXTrigger : MonoBehaviour
{
    [Header("Аудиоменеджер")]
    public AudioManager audioManager;
    [Header("Короткий звук")]
    public AudioClip sfxClip;

    public void TriggerSFX()
    {
        audioManager.PlaySFX(sfxClip);
    }
}