using NavMeshPlus.Components;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavMeshTestScr : MonoBehaviour
{
    
    [SerializeField] NavMeshAgent testAgent;
    [SerializeField] NavMeshSurface surface;
    [SerializeField] WorldFill wfill;
    GameObject target;

    bool flag = false;

    // Update is called once per frame
    void Update()
    {
        if (wfill.placesSpawned && !flag)
        {
            surface.BuildNavMeshAsync();
            testAgent.updateRotation = false;
            testAgent.updateUpAxis = false;
            flag = true;
        }
        if (!flag) return;
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player");
            return;
        }
        testAgent.SetDestination(target.transform.position);
    }
}
