using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTestScr : MonoBehaviour
{
    
    [SerializeField] NavMeshSurface surface;
    [SerializeField] WorldFill wfill;

    bool flag = false;

    // Update is called once per frame
    void Update()
    {
        if (wfill.placesSpawned && !flag)
        {
            surface.BuildNavMeshAsync();
            flag = true;
        }
    }
}
