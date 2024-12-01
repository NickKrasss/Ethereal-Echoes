using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicComponent : MonoBehaviour
{
    private List<Action> musicActions;

    void Start()
    {
        var audioSource = gameObject.GetComponent<AudioSource>();
        musicActions = new List<Action>() 
        { 
            () => AudioManager.Instance.PlayMusicOnTheFirstLevel("1_1", audioSource),
            () => AudioManager.Instance.PlayMusicOnTheFirstLevel("1_2", audioSource)
        };




    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
