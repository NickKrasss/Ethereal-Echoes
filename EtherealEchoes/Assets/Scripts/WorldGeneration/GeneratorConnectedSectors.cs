using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


[RequireComponent(typeof(WorldFill))]
public class GeneratorConnectedSectors : MonoBehaviour
{
    

    [Tooltip("Размер центрального сектора. Минимальная ширина, Минимальная высота, Максимальная ширина, Максимальная высота")]
    [SerializeField]
    private int[] startSectorSize = { 1, 1, 2, 2 };

    [Tooltip("Количество секторов (не включая стартовый). Минимум, максимум")]
    [SerializeField]
    private int[] sectorsCount = { 1, 1 };

    [Tooltip("Размер секторов. Минимальная ширина, Минимальная высота, Максимальная ширина, Максимальная высота")]
    [SerializeField]
    private int[] sectorsSize = { 1, 1, 2, 2 };

    [Tooltip("Чем меньше, тем шире границы.")]
    [SerializeField]
    private float cellChance = 0.45f;

    [Tooltip("Количество шагов \"сглаживания\".")]
    [SerializeField]
    private int steps = 50;

    private List<Sector> sectors = new List<Sector>();

    public WorldFill worldScr;

    private void Awake()
    {
        worldScr = GetComponent<WorldFill>();
    }
    private void Start()
    {
        GenerateWorld();
    }

    private Sector CreateStartSector(int[] center)
    {
        int[] startSize = { UnityEngine.Random.Range(startSectorSize[0], startSectorSize[2]), UnityEngine.Random.Range(startSectorSize[1], startSectorSize[3]) };
        int x1 = center[0] - startSize[0] / 2;
        int y1 = center[1] - startSize[1] / 2;

        int x2 = center[0] + startSize[0] / 2;
        int y2 = center[1] + startSize[1] / 2;
        while (!worldScr.isSectorEmpty(x1, y1, x2, y2))
        {
            startSize[0] = UnityEngine.Random.Range(startSectorSize[0], startSectorSize[2]);
            startSize[1] = UnityEngine.Random.Range(startSectorSize[1], startSectorSize[3]);
            x1 = center[0] - startSize[0] / 2;
            y1 = center[1] - startSize[1] / 2;

            x2 = center[0] + startSize[0] / 2;
            y2 = center[1] + startSize[1] / 2;
        }
        return new Sector(x1, y1, x2, y2);
    }

    private Sector GenerateSector()
    {
        int tries = 0;
        while (true)
        {
            tries++;
            if (tries >= 1000)
            {
                Debug.Log("Не помещается!!!!");
                return null;
            }
            Sector baseSector = sectors[UnityEngine.Random.Range(0, sectors.Count)];
            int[] size = { UnityEngine.Random.Range(sectorsSize[0], sectorsSize[2]), UnityEngine.Random.Range(sectorsSize[1], sectorsSize[3]) };
            switch (UnityEngine.Random.Range(0, 4))
            {
                case 0: // Вверх
                    {
                        int x = (int)UnityEngine.Random.Range(baseSector.x1 + baseSector.width * 0.15f, baseSector.x2 - baseSector.width * 0.15f);
                        if (x < baseSector.center[0])
                        {
                            int x1 = x - size[0];
                            int y1 = baseSector.y2;
                            int x2 = x;
                            int y2 = y1 + size[1];
                            if (worldScr.isSectorEmpty(x1, y1, x2, y2))
                                return new Sector(x1, y1, x2, y2);
                        }
                        else
                        {
                            int x1 = x;
                            int y1 = baseSector.y2;
                            int x2 = x + size[0];
                            int y2 = y1 + size[1];
                            if (worldScr.isSectorEmpty(x1, y1, x2, y2))
                                return new Sector(x1, y1, x2, y2);
                        }
                        break;
                    }
                case 1: // Вправо
                    {
                        int y = (int)UnityEngine.Random.Range(baseSector.y1 + baseSector.height * 0.15f, baseSector.y2 - baseSector.height * 0.15f);
                        if (y < baseSector.center[1])
                        {
                            int x1 = baseSector.x2;
                            int y2 = y;
                            int x2 = x1 + size[0];
                            int y1 = y2 - size[1];
                            if (worldScr.isSectorEmpty(x1, y1, x2, y2))
                                return new Sector(x1, y1, x2, y2);
                        }
                        else
                        {
                            int x1 = baseSector.x2;
                            int y1 = y;
                            int x2 = x1 + size[0];
                            int y2 = y1 + size[1];
                            if (worldScr.isSectorEmpty(x1, y1, x2, y2))
                                return new Sector(x1, y1, x2, y2);
                        }
                        break;
                    }
                case 2: // Влево
                    {
                        int y = (int)UnityEngine.Random.Range(baseSector.y1 + baseSector.height * 0.15f, baseSector.y2 - baseSector.height * 0.15f);
                        if (y < baseSector.center[1])
                        {
                            int x2 = baseSector.x1;
                            int y2 = y;
                            int x1 = x2 - size[0];
                            int y1 = y2 - size[1];
                            if (worldScr.isSectorEmpty(x1, y1, x2, y2))
                                return new Sector(x1, y1, x2, y2);
                        }
                        else
                        {
                            int x2 = baseSector.x1;
                            int y1 = y;
                            int x1 = x2 - size[0];
                            int y2 = y1 + size[1];
                            if (worldScr.isSectorEmpty(x1, y1, x2, y2))
                                return new Sector(x1, y1, x2, y2);
                        }
                        break;
                    }
                case 3: // Вниз
                    {
                        int x = (int)UnityEngine.Random.Range(baseSector.x1 + baseSector.width * 0.15f, baseSector.x2 - baseSector.width * 0.15f);
                        if (x < baseSector.center[0])
                        {
                            int x1 = x - size[0];
                            int y2 = baseSector.y1;
                            int x2 = x;
                            int y1 = y2 - size[1];
                            if (worldScr.isSectorEmpty(x1, y1, x2, y2))
                                return new Sector(x1, y1, x2, y2);
                        }
                        else
                        {
                            int x1 = x;
                            int y2 = baseSector.y1;
                            int x2 = x + size[0];
                            int y1 = y2 - size[1];
                            if (worldScr.isSectorEmpty(x1, y1, x2, y2))
                                return new Sector(x1, y1, x2, y2);
                        }
                        break;
                    }
            }
        }
    }

    private void GenerateSectors(int[] center)
    {
        Sector start = CreateStartSector(center);
        start.Fill(worldScr);
        sectors.Add(start);
        int count = UnityEngine.Random.Range(sectorsCount[0], sectorsCount[1]);

        for (int i = 0; i < count; i++)
        {
            Sector sector = GenerateSector();
            sector.Fill(worldScr);
            sectors.Add(sector);
        }
    }

    public void BorderStep()
    {
        int[,] worldCopy = new int[worldScr.width, worldScr.height];
        for (int x = 1; x < worldScr.width - 1; x++)
        {
            for (int y = 1; y < worldScr.height - 1; y++)
            {
                worldCopy[x, y] = worldScr.world[x, y];
                if (worldScr.world[x, y] == 2) continue;
                int cellsNearby = 0;
                for (int xx = -1; xx <= 1; xx++)
                {
                    for (int yy = -1; yy <= 1; yy++)
                    {
                        if (xx == 0 && yy == 0) continue;
                        if (worldScr.world[x + xx, y + yy] != 0) cellsNearby++;
                    }
                }

                if (worldScr.world[x, y] != 0)
                {
                    if (cellsNearby <= 3) worldCopy[x, y] = 0;
                }
                else if (cellsNearby >= 4) worldCopy[x, y] = 1;
            }
        }
        worldScr.world = worldCopy;
    }

    private void GenerateBorders(int steps)
    {
        for (int x = 1; x < worldScr.width - 1; x++)
        {
            for (int y = 1; y < worldScr.height - 1; y++)
            {
                if (worldScr.world[x, y] == 2) continue;
                worldScr.world[x, y] = UnityEngine.Random.Range(0, 1f) < cellChance ? 1 : 0;
            }
        }
        for (int i = 0; i < steps; i++)
        {
            BorderStep();
        }
    }

    public void DeleteOtherTiles()
    {
        int[] center = { worldScr.width / 2, worldScr.height / 2 };

        Queue<Tuple<int, int>> queue = new Queue<Tuple<int, int>>();
        queue.Enqueue(new Tuple<int, int>(center[0], center[1]));
        while (queue.Any())
        {
            Tuple<int, int> point = queue.Dequeue();
            if (worldScr.world[point.Item1, point.Item2] != 1 && worldScr.world[point.Item1, point.Item2] != 2)
                continue;
            worldScr.world[point.Item1, point.Item2] = 3;
            EnqueueIfMatches(worldScr.world, queue, point.Item1 - 1, point.Item2);
            EnqueueIfMatches(worldScr.world, queue, point.Item1 + 1, point.Item2);
            EnqueueIfMatches(worldScr.world, queue, point.Item1, point.Item2 - 1);
            EnqueueIfMatches(worldScr.world, queue, point.Item1, point.Item2 + 1);
        }
        for (int x = 0; x < worldScr.width; x++)
        {
            for (int y = 0; y < worldScr.height; y++)
            {
                if (worldScr.world[x, y] != 3)
                    worldScr.world[x, y] = 0;
                else
                    worldScr.world[x, y] = 1;
            }
        }
    }
    private void EnqueueIfMatches(int[,] array, Queue<Tuple<int, int>> queue, int x, int y)
    {
        if (x < 0 || x >= array.GetLength(0) || y < 0 || y >= array.GetLength(1))
            return;
        if (array[x, y] == 1 || array[x, y] == 2)
            queue.Enqueue(new Tuple<int, int>(x, y));
    }

    public void GenerateWorld()
    {
        sectors = new List<Sector>();
        worldScr.world = new int[worldScr.width, worldScr.height];
        int[] center = { worldScr.width / 2, worldScr.height / 2 };
        GenerateSectors(center);

        GenerateBorders(steps);

        DeleteOtherTiles();
        worldScr.DrawEverything();
        worldScr.generated = true;
    }
   
}

[CustomEditor(typeof(GeneratorConnectedSectors))]
public class SectorGenUI : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        GeneratorConnectedSectors gen = (GeneratorConnectedSectors)target;
        if (GUILayout.Button("Пересоздать"))
        {
            gen.GenerateWorld();
        }
        if (GUILayout.Button("Шаг сглаживания"))
        {
            gen.BorderStep();
            gen.worldScr.DrawEverything();
            gen.worldScr.generated = true;
        }
    }
}