using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(WorldDraw))]
public class WorldFill : MonoBehaviour
{
    [Tooltip("������ �����")]
    [SerializeField]
    public int width = 1;

    [Tooltip("������ �����")]
    [SerializeField]
    public int height = 1;

    // ������� "���������" ����, 1 - ��������� ������.   
    public int[,] world;

    [HideInInspector]
    public bool generated = false;
    [HideInInspector]
    public bool placesSpawned = false;

    private void Awake()
    {
        world = new int[width, height];
    }

    // �������� ��� ����� �����
    public void DrawEverything()
    {
        GetComponent<WorldDraw>().DrawEverything();
    }

    
}
