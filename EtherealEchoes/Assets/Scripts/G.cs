using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G : MonoBehaviour
{
    public static G Instance { get; private set; }

    public World currentWorld;
    public GameObject currentWorldObj;
    public GameObject playerObj;

    public int currentLevel = 0;

    public int currentTime = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
