using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlaceGenerator
{
    public (bool, int[,], Place[]) GeneratePlaces(int[,] map, List<(int, int)> clearPoints = null);
    public void ClearPlaces();
}