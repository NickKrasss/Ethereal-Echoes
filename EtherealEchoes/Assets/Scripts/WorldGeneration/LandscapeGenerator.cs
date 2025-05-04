using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface LandscapeGenerator
{
    public int[,] GenerateLandscape(int width, int height);

    public List<(int, int)> getClearPoints();
}