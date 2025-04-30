using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G : MonoBehaviour
{
    public static G Instance { get; private set; }

    public World currentWorld;
    public GameObject currentWorldObj;
    public GameObject playerObj;

    public Light gameLight;

    public int currentLevel = 0;

    public int currentTime = 0;

    public bool playerDead = false;

    public bool isWorldLoading = false;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
