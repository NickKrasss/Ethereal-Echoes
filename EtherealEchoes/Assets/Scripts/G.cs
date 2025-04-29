using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G : MonoBehaviour
{
    public static G Instance { get; private set; }

    public World currentWorld;
    public GameObject currentWorldObj;
    public GameObject playerObj;

    public List<string> powerUpCards;
    public int currentLevel = 1;
    public float criticalHitChance = 0;

    public int countEqualsStringNames(List<string> stringNames, string name)
    {
        int count = 0;
        foreach (var names in stringNames)
        {
            if (names == name)
            {
                count++;
            }
        }

        return count;
    }

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
