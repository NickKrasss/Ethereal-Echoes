using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTestScr : MonoBehaviour
{
    
    NavMeshSurface surface;


    private void Awake()
    {
        surface = GetComponent<NavMeshSurface>();
    }

    public void Build()
    {
        surface.BuildNavMeshAsync();
    }
}
