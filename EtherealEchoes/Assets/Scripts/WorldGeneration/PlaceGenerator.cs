using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface PlaceGenerator
{
    public (int[,], Place[]) GeneratePlaces(int[,] map);
}