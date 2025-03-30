using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] clips;

    AudioSource source = null;

    private void Start()
    {
    }

    void Update()
    {
        if (clips.Length == 0 || !AudioManager.Instance) return;
        if (source == null)
            source = AudioManager.Instance.PlayAudio(clips[Random.Range(0, clips.Length)], SoundType.Music);
        AudioManager.Instance.UpdateVolume(source, SoundType.Music);
    }
}
