using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(WorldDraw))]
public class WorldFill : MonoBehaviour
{
    [Tooltip("Ширина карты")]
    [SerializeField]
    public int width = 1;

    [Tooltip("Высота карты")]
    [SerializeField]
    public int height = 1;

    // Матрица "занятости" мира, 1 - свободная клетка.   
    public int[,] world;

    [HideInInspector]
    public bool generated = false;
    [HideInInspector]
    public bool placesSpawned = false;

    private void Awake()
    {
        world = new int[width, height];
    }

    // Отрисует всю карту сразу
    public void DrawEverything()
    {
        GetComponent<WorldDraw>().DrawEverything();
    }

    
}
