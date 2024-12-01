using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Music : MonoBehaviour
{
    private AudioSource source;
    [SerializeField]
    private AudioClip[] clips;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (clips.Length == 0 || !AudioManager.Instance) return;
        if (!source.isPlaying)
            AudioManager.Instance.PlayAudio(source, clips[Random.Range(0, clips.Length)], SoundType.Music);
        AudioManager.Instance.UpdateVolume(source, SoundType.Music);
    }
}
