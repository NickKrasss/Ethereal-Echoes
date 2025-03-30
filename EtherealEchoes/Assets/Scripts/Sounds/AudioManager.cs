using UnityEngine;
using UnityEngine.UI; // Для работы с UI и слайдерами
using System.Collections.Generic;
using UnityEngine.Rendering;
using UnityEngine.Rendering.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

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
            if (source == null)
            { 
                audioSources.Remove(source);
                break;
            }
            if (!source.isPlaying)
            {
                Destroy(source.gameObject);
                audioSources.Remove(source);
                break;
            }
        }
    }

    
}

public enum SoundType
{ 
    SFX, Music
}
