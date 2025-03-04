using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    
    
}


