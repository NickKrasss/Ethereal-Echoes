using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioLibrary", menuName = "Audio/Audio Library")]
public class AudioLibrary : ScriptableObject
{
    public List<AudioEntry> audioClips;

    public AudioClip GetClip(string name)
    {
        foreach (var entry in audioClips)
        {
            if (entry.name == name)
                return entry.clip;
        }

        return null;
    }
}

[System.Serializable]
public class AudioEntry
{
    public string name;
    public AudioClip clip;
}
