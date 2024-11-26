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

    // Проверка, что клетка находится в границе мира
    public bool isCellInBorders(int x, int y)
    { 
        return x >= 0 && y >= 0 && x < width && y < height;
    }

    // Проверка, что сектор находится в границе мира
    public bool isSectorInBorders(int x1, int y1, int x2, int y2)
    {
        return isCellInBorders(x1, y1) && isCellInBorders(x2, y2);
    }

    // Проверка, что сектор находится в границе мира
    public bool isSectorInBorders(Sector sector)
    {
        return isCellInBorders(sector.x1, sector.y1) && isCellInBorders(sector.x2, sector.y2);
    }

    // Проверка, что в секторе все значения равны i
    public bool isSectorEmpty(int x1, int y1, int x2, int y2, int i = 0)
    {
        if (!isSectorInBorders(x1, y1, x2, y2)) 
            return false;

        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                if (world[x, y] != i)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // Проверка, что в секторе все значения равны i
    public bool isSectorEmpty(Sector sector, int i = 0)
    {
        if (sector == null) return false;
        if (!isSectorInBorders(sector.x1, sector.y1, sector.x2, sector.y2))
            return false;
        for (int x = sector.x1; x <= sector.x2; x++)
        {
            for (int y = sector.y1; y <= sector.y2; y++)
            {
                if (world[x, y] != i)
                {
                    return false;
                }
            }
        }
        return true;
    }
}

// Сектор - просто прямоугольник, который хранит левый нижний и правый верхний угол. Есть также центр, ширина и высота.
// Удобно при генерации мира
public class Sector
{
    public int x1;
    public int y1;
    public int x2;
    public int y2;

    public float[] center;
    public int width;
    public int height;

    public Sector(int x1, int y1, int x2, int y2)
    {
        this.x1 = x1; this.y1 = y1;
        this.x2 = x2; this.y2 = y2;
        width = x2 - x1;
        height = y2 - y1;
        center = new float[] { x1 + width / 2.0f, y1 + height / 2.0f };
    }
    public Sector(float[] center, int width, int height)
    {
        x1 = (int)center[0] - width / 2; y1 = (int)center[1] - height / 2;
        x2 = x1 + width; y2 = y1 + width;
        this.width = width;
        this.height = height;
        this.center = center;
    }

    /// <summary>
    /// Заполнить сектор в world числом fillDigit
    /// </summary>
    /// <param name="worldScr"></param>
    /// <param name="fillDigit"></param>
    /// <returns></returns>
    public bool Fill(WorldFill worldScr, int fillDigit = 2)
    {
        if (!worldScr.isSectorInBorders(x1, y1, x2, y2)) { return false; }
        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                worldScr.world[x, y] = fillDigit;
            }
        }
        return true;
    }
}
