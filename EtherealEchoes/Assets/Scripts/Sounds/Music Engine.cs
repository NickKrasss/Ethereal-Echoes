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

    [Header("Слайдер для музыки")]
    public Slider soundtrackVolumeSlider;

    private int currentTrackIndex = -1;
    private float soundtrackVolume = 1.0f;
    private bool isMuted = false;

    private void Start()
    {
        if (soundtrackSource == null)
        {
            Debug.LogError("AudioSource для soundtrackSource не назначен!");
            return;
        }

        if (soundtracks.Count == 0)
        {
            Debug.LogError("Список саундтреков пуст!");
            return;
        }

        soundtrackVolume = PlayerPrefs.GetFloat("SoundtrackVolume", 1.0f);
        soundtrackVolumeSlider.value = soundtrackVolume;
        soundtrackSource.volume = soundtrackVolume;

        soundtrackVolumeSlider.onValueChanged.AddListener(SetSoundtrackVolume);

        StartCoroutine(PlayRandomSoundtrack());
    }

    private void SetSoundtrackVolume(float value)
    {
        soundtrackVolume = value;
        soundtrackSource.volume = value;
        PlayerPrefs.SetFloat("SoundtrackVolume", value);
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
}
