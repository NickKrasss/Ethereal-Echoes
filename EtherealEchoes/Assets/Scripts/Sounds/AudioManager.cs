using UnityEngine;
using UnityEngine.UI; // Для работы с UI и слайдерами
using System.Collections.Generic;
using UnityEngine.Rendering;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioSource musicSource; // Источник для музыки
    [SerializeField] public AudioSource sfxSource; // Источник для SFX
    [SerializeField] private AudioLibrary audioLibrary;

    [SerializeField] private Dictionary<GameObject, AudioSource> mobAudioSources = new Dictionary<GameObject, AudioSource>(); // Для мобов
    [SerializeField] private Dictionary<GameObject, AudioSource> uiAudioSources = new Dictionary<GameObject, AudioSource>(); // Для UI объектов

    [SerializeField] private List<GameObject> uiObjectsToRegister = new List<GameObject>(); // Список для UI объектов
    [SerializeField] private List<GameObject> mobsToRegister = new List<GameObject>(); // Список для мобов

    [Header("Слайдер громкости музыки")]
    [SerializeField] private Slider musicVolumeSlider; // Слайдер громкости музыки
    [Header("Слайдер громкости звуков")]
    [SerializeField] private Slider soundVolumeSlider; // Слайдер громкости звуков

    private float timer = 0f; // Таймер для отслеживания времени
    private float interval = 1f; // Интервал времени в секундах

    private float currentVolume = 1.0f; // Текущая громкость музыки

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); // Чтобы менеджер сохранялся между сценами

        // Если musicSource не назначен, ищем компонент в сцене
        if (musicSource == null)
        {
            FindMusicSource();
        }

        if (musicSource == null || sfxSource == null)
        {
            Debug.Log("AudioManager: AudioSource components are not assigned!");
        }

        // Регистрируем UI объекты и мобы из списков
        RegisterUIObjects();
        RegisterMobs();

        // Загружаем громкость из PlayerPrefs, если она есть, и устанавливаем слайдер
        currentVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f); 
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.value = currentVolume;
        }

        // Подписываемся на изменение значения слайдера
        if (musicVolumeSlider != null)
        {
            musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        currentVolume = PlayerPrefs.GetFloat("SoundsVolume", 1.0f);
        if (soundVolumeSlider != null)
        {
            soundVolumeSlider.value = currentVolume;
        }

        // Подписываемся на изменение значения слайдера
        if (soundVolumeSlider != null)
        {
            soundVolumeSlider.onValueChanged.AddListener(OnSoundVolumeChanged);
        }
    }

    // Метод для поиска источника музыки с именем, начинающимся с "Main"
    private void FindMusicSource()
    {
        // Ищем все объекты в сцене с компонентом AudioSource
        AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
        foreach (var source in audioSources)
        {
            // Проверяем, начинается ли имя объекта с "Main"
            if (source.gameObject.name.StartsWith("Main"))
            {
                musicSource = source;
                Debug.Log("AudioManager: Found music source: " + source.gameObject.name);
                break;
            }
        }
    }

    // Метод для проигрывания музыки
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
        musicSource.volume = PlayerPrefs.GetFloat("MusicVolume", 1.0f); // Громкость, полученная через слайдер
        musicSource.loop = true; // Музыка будет зациклена
        musicSource.Play();
    }

    // Метод для проигрывания звуков (SFX)
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

    // Остановка музыки
    public void StopMusic()
    {
        if (musicSource == null)
        {
            Debug.LogError("AudioManager: Music source is missing!");
            return;
        }

        musicSource.Stop();
    }

    // Удаление "мусорных" ссылок мобов
    private void CleanupDestroyedObjects()
    {
        CleanupDestroyedMobs();
        CleanupDestroyedUIObjects();
    }

    // Удаление "мусорных" ссылок мобов
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

    // Удаление "мусорных" ссылок UI объектов
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

    // Регистрация UI объектов
    private void RegisterUIObjects()
    {
        foreach (var uiObject in uiObjectsToRegister)
        {
            RegisterUIObject(uiObject);
        }
    }

    // Регистрация мобов
    private void RegisterMobs()
    {
        foreach (var mob in mobsToRegister)
        {
            RegisterMob(mob);
        }
    }

    // Регистрация мобов
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

    // Удаление мобов
    public void UnregisterMob(GameObject mob)
    {
        if (mob == null) return;

        if (mobAudioSources.ContainsKey(mob))
        {
            mobAudioSources.Remove(mob);
        }
    }

    // Проигрывание звука моба
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

    // Регистрация UI объектов
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

    // Удаление UI объектов
    public void UnregisterUIObject(GameObject uiObject)
    {
        if (uiObject == null) return;

        if (uiAudioSources.ContainsKey(uiObject))
        {
            uiAudioSources.Remove(uiObject);
        }
    }

    // Проигрывание звука UI объекта
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
        // Обновляем громкость музыки каждые 0.01 секунды
        if (musicSource != null)
        {
            musicSource.volume = Mathf.Lerp(musicSource.volume, currentVolume, 0.1f); // Плавное обновление громкости
        }
    }

    // Обработчик изменения громкости музыки
    private void OnMusicVolumeChanged(float volume)
    {
        currentVolume = volume; // Сохраняем новое значение громкости
        PlayerPrefs.SetFloat("MusicVolume", volume); // Сохраняем значение в PlayerPrefs
        PlayerPrefs.Save(); // Сохраняем изменения
    }

    // Обработчик изменения громкости звуков
    private void OnSoundVolumeChanged(float volume)
    {
        currentVolume = volume; // Сохраняем новое значение громкости
        PlayerPrefs.SetFloat("SoundsVolume", volume); // Сохраняем значение в PlayerPrefs
        PlayerPrefs.Save(); // Сохраняем изменения
    }
}
