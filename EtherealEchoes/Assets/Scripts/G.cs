using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class G : MonoBehaviour
{
    public static G Instance { get; private set; }

    public World currentWorld;
    public GameObject currentWorldObj;
    public GameObject playerObj;
    public PowerUpCardsController powerUpCardsController;


    public Light gameLight;
    public float blockDamageChance = 0;
    public float extraGearsOffset = 0;
    public int currentLevel = 1;
    public double criticalHitChance = 0;
    public double criticalHitAmount = 1.75;

    public List<string> powerUpCards;
    public List<int> dropChancesCommonChest = new List<int> { 70, 90, 100 };
    public List<int> dropChancesRareChest = new List<int> { 5, 99, 130 };
    public List<int> dropChancesStatsPlace = new List<int> { 75, 240, 240 };
    public List<int> dropChancesArtifactPlace = new List<int> { 20, 50, 100 };
    public List<int> dropChancesBoss = new List<int> { 0, 0, 100 };

     

    //public int currentLevel = 0;

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

        if (gameLight == null)
        {
            gameLight = GameObject.FindWithTag("MainLight").GetComponent<Light>();
        }

    }
}
