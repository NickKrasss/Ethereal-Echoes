using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class World
{
    // ћатрица "зан€тости" мира, 0 - свободна€ клетка. 1 - стена. 2 - обьект. 3 - вода.
    private int[,] map;
    private Place[] places;

    private int width;
    private int height;

    private LandscapeGenerator landGen;
    private PlaceGenerator placeGen;

    public int[,] Map {  get { return map; } }
    public int Width { get { return width; } }
    public int Height { get { return height; } }
    public LandscapeGenerator LandGenerator { get { return landGen; } }
    public PlaceGenerator PlaceGenerator { get { return placeGen; } }

    public World(int width, int height, LandscapeGenerator landGen, PlaceGenerator placeGen)
    {
        this.width = width;
        this.height = height;
        this.landGen = landGen;
        this.placeGen = placeGen;
        this.map = new int[width, height];
    }

    public void GenerateWorld()
    {
        this.map = landGen.GenerateLandscape(width, height);

        var placesResult = placeGen.GeneratePlaces(map, landGen.getClearPoints());
        while (!placesResult.Item1)
        {
            placesResult = placeGen.GeneratePlaces(map, landGen.getClearPoints());
        }
        this.map = placesResult.Item2;
        this.places = placesResult.Item3;
    }

    public static bool Fill(int[,] landscape, Sector sector, int fillDigit = 1)
    {
        if (!isSectorInBorders(landscape, sector)) { return false; }
        for (int x = sector.x1; x <= sector.x2; x++)
        {
            for (int y = sector.y1; y <= sector.y2; y++)
            {
                landscape[x, y] = fillDigit;
            }
        }
        return true;
    }

    // ѕроверка, что клетка находитс€ в границе мира
    public static bool isCellInBorders(int[,] landscape, int x, int y)
    {
        return x >= 0 && y >= 0 && x < landscape.GetLength(0) && y < landscape.GetLength(1);
    }

    // ѕроверка, что сектор находитс€ в границе мира
    public static bool isSectorInBorders(int[,] landscape, int x1, int y1, int x2, int y2)
    {
        return isCellInBorders(landscape, x1, y1) && isCellInBorders(landscape, x2, y2);
    }

    // ѕроверка, что сектор находитс€ в границе мира
    public static bool isSectorInBorders(int[,] landscape, Sector sector)
    {
        return isCellInBorders(landscape, sector.x1, sector.y1) && isCellInBorders(landscape, sector.x2, sector.y2);
    }

    // ѕроверка, что в секторе все значени€ равны i
    public static bool isSectorFilledWith(int[,] landscape, int x1, int y1, int x2, int y2, int i)
    {
        if (!isSectorInBorders(landscape, x1, y1, x2, y2))
            return false;

        for (int x = x1; x <= x2; x++)
        {
            for (int y = y1; y <= y2; y++)
            {
                if (landscape[x, y] != i)
                {
                    return false;
                }
            }
        }
        return true;
    }

    // ѕроверка, что в секторе все значени€ равны i
    public static bool isSectorFilledWith(int[,] landscape, Sector sector, int i)
    {
        if (sector == null) return false;
        if (!isSectorInBorders(landscape, sector.x1, sector.y1, sector.x2, sector.y2))
            return false;
        for (int x = sector.x1; x <= sector.x2; x++)
        {
            for (int y = sector.y1; y <= sector.y2; y++)
            {
                if (landscape[x, y] != i)
                {
                    return false;
                }
            }
        }
        return true;
    }

}