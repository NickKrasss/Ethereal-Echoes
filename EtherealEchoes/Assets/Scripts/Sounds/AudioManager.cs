using UnityEngine;
using UnityEngine.UI; // ��� ������ � UI � ����������
using System.Collections.Generic;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource; // �������� ��� ������
    [SerializeField] public AudioSource sfxSource; // �������� ��� SFX
    [SerializeField] private AudioLibrary audioLibrary;

    [SerializeField] private Dictionary<GameObject, AudioSource> mobAudioSources = new Dictionary<GameObject, AudioSource>(); // ��� �����
    [SerializeField] private Dictionary<GameObject, AudioSource> uiAudioSources = new Dictionary<GameObject, AudioSource>(); // ��� UI ��������

    [SerializeField] private List<GameObject> uiObjectsToRegister = new List<GameObject>(); // ������ ��� UI ��������
    [SerializeField] private List<GameObject> mobsToRegister = new List<GameObject>(); // ������ ��� �����

    [Header("������� ��������� ������")]
    [SerializeField] private Slider musicVolumeSlider; // ������� ��������� ������
    [Header("������� ��������� ������")]
    [SerializeField] private Slider soundVolumeSlider; // ������� ��������� ������

    private float timer = 0f; // ������ ��� ������������ �������
    private float interval = 1f; // �������� ������� � ��������

    private float currentVolume = 1.0f; // ������� ��������� ������

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // ����� �������� ���������� ����� �������

        // ���� musicSource �� ��������, ���� ��������� � �����
        if (musicSource == null)
        {
            FindMusicSource();
        }

        if (musicSource == null || sfxSource == null)
        {
            Debug.Log("AudioManager: AudioSource components are not assigned!");
        }

        // ������������ UI ������� � ���� �� �������
        RegisterUIObjects();
        RegisterMobs();

        // ��������� ��������� �� PlayerPrefs, ���� ��� ����, � ������������� �������
        currentVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f); 
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = currentVolume;
        }

        // ������������� �� ��������� �������� ��������
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        currentVolume = PlayerPrefs.GetFloat("SoundsVolume", 1.0f);
        if (soundVolumeSlider != null)
        {
            soundVolumeSlider.value = currentVolume;
        }

        // ������������� �� ��������� �������� ��������
        if (soundVolumeSlider != null)
        {
            soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        }
    }

    // ����� ��� ������ ��������� ������ � ������, ������������ � "Main"
    private void FindMusicSource()
    {
        // ���� ��� ������� � ����� � ����������� AudioSource
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (var source in audioSources)
        {
            // ���������, ���������� �� ��� ������� � "Main"
            if (source.gameObject.name.StartsWith("Main"))
            {
                musicSource = source;
                Debug.Log("AudioManager: Found music source: " + source.gameObject.name);
                break;
            }
        }
    }

    // ����� ��� ������������ ������
    public void PlayMusic(string musicName, float volume = 1.0f)
    {
        if (musicSource == null)
        {
            Debug.LogError("AudioManager: Music source is missing!");
            return;
        }

        AudioClip clip = audioLibrary.GetClip(musicName);
        if (clip == null)
        {
            Debug.LogWarning($"Music clip '{musicName}' not found!");
            return;
        }

        musicSource.clip = clip;
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f); // ���������, ���������� ����� �������
        musicSource.loop = true; // ������ ����� ���������
        musicSource.Play();
    }

    // ����� ��� ������������ ������ (SFX)
    public void PlaySound(string soundName, AudioSource audiosource)
    {
        sfxSource = audiosource;

        if (sfxSource == null)
        {
            Debug.LogError("AudioManager: SFX source is missing!");
            return;
        }

        AudioClip clip = audioLibrary.GetClip(soundName);
        if (clip == null)
        {
            Debug.LogWarning($"Sound clip '{soundName}' not found!");
            return;
        }

        var myVolume = PlayerPrefs.GetFloat("SoundsVolume", 1.0f);
        sfxSource.PlayOneShot(clip, myVolume);
    }

    // ��������� ������
    public void StopMusic()
    {
        if (musicSource == null)
        {
            Debug.LogError("AudioManager: Music source is missing!");
            return;
        }

        musicSource.Stop();
    }

    // �������� "��������" ������ �����
    private void CleanupDestroyedObjects()
    {
        CleanupDestroyedMobs();
        CleanupDestroyedUIObjects();
    }

    // �������� "��������" ������ �����
    private void CleanupDestroyedMobs()
    {
        List<GameObject> mobsToRemove = new List<GameObject>();

        foreach (var entry in mobAudioSources)
        {
            if (entry.Key == null || entry.Value == null)
            {
                mobsToRemove.Add(entry.Key);
            }
        }

        foreach (var mob in mobsToRemove)
        {
            mobAudioSources.Remove(mob);
        }
    }

    // �������� "��������" ������ UI ��������
    private void CleanupDestroyedUIObjects()
    {
        List<GameObject> uiObjectsToRemove = new List<GameObject>();

        foreach (var entry in uiAudioSources)
        {
            if (entry.Key == null || entry.Value == null)
            {
                uiObjectsToRemove.Add(entry.Key);
            }
        }

        foreach (var uiObject in uiObjectsToRemove)
        {
            uiAudioSources.Remove(uiObject);
        }
    }

    // ����������� UI ��������
    private void RegisterUIObjects()
    {
        foreach (var uiObject in uiObjectsToRegister)
        {
            RegisterUIObject(uiObject);
        }
    }

    // ����������� �����
    private void RegisterMobs()
    {
        foreach (var mob in mobsToRegister)
        {
            RegisterMob(mob);
        }
    }

    // ����������� �����
    public void RegisterMob(GameObject mob)
    {
        if (mob == null) return;

        if (!mobAudioSources.ContainsKey(mob))
        {
            AudioSource mobAudioSource = mob.GetComponent<AudioSource>();
            if (mobAudioSource == null)
            {
                mobAudioSource = mob.AddComponent<AudioSource>();
            }

            mobAudioSources[mob] = mobAudioSource;
        }
    }

    // �������� �����
    public void UnregisterMob(GameObject mob)
    {
        if (mob == null) return;

        if (mobAudioSources.ContainsKey(mob))
        {
            mobAudioSources.Remove(mob);
        }
    }

    // ������������ ����� ����
    public void PlayMobSound(GameObject mob, string soundName, float volume = 1.0f)
    {
        if (mob == null || !mobAudioSources.ContainsKey(mob))
        {
            Debug.LogWarning($"AudioManager: Mob '{mob?.name}' is not registered or has been destroyed!");
            return;
        }

        AudioClip clip = audioLibrary.GetClip(soundName);
        if (clip == null)
        {
            Debug.LogWarning($"AudioManager: Sound clip '{soundName}' not found!");
            return;
        }

        AudioSource mobSource = mobAudioSources[mob];
        mobSource.PlayOneShot(clip, volume);
    }

    // ����������� UI ��������
    public void RegisterUIObject(GameObject uiObject)
    {
        if (uiObject == null) return;

        if (!uiAudioSources.ContainsKey(uiObject))
        {
            AudioSource uiAudioSource = uiObject.GetComponent<AudioSource>();
            if (uiAudioSource == null)
            {
                uiAudioSource = uiObject.AddComponent<AudioSource>();
            }

            uiAudioSources[uiObject] = uiAudioSource;
        }
    }

    // �������� UI ��������
    public void UnregisterUIObject(GameObject uiObject)
    {
        if (uiObject == null) return;

        if (uiAudioSources.ContainsKey(uiObject))
        {
            uiAudioSources.Remove(uiObject);
        }
    }

    // ������������ ����� UI �������
    public void PlayUISound(GameObject uiObject, string soundName, float volume = 1.0f)
    {
        if (uiObject == null || !uiAudioSources.ContainsKey(uiObject))
        {
            Debug.LogWarning($"AudioManager: UI Object '{uiObject?.name}' is not registered or has been destroyed!");
            return;
        }

        AudioClip clip = audioLibrary.GetClip(soundName);
        if (clip == null)
        {
            Debug.LogWarning($"AudioManager: Sound clip '{soundName}' not found!");
            return;
        }

        AudioSource uiSource = uiAudioSources[uiObject];
        uiSource.PlayOneShot(clip, volume);
    }

    private void Update()
    {
        // ��������� ��������� ������ ������ 0.01 �������
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Lerp(musicSource.volume, currentVolume, 0.1f); // ������� ���������� ���������
        }
    }

    // ���������� ��������� ��������� ������
    private void OnMusicVolumeChanged(float volume)
    {
        currentVolume = volume; // ��������� ����� �������� ���������
        PlayerPrefs.SetFloat("MusicVolume", volume); // ��������� �������� � PlayerPrefs
        PlayerPrefs.Save(); // ��������� ���������
    }

    // ���������� ��������� ��������� ������
    private void OnSoundVolumeChanged(float volume)
    {
        currentVolume = volume; // ��������� ����� �������� ���������
        PlayerPrefs.SetFloat("SoundsVolume", volume); // ��������� �������� � PlayerPrefs
        PlayerPrefs.Save(); // ��������� ���������
    }
}
