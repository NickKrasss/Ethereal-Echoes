using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableAtRangeScr : MonoBehaviour
{

    [SerializeField]
    private string targetTag;

    [SerializeField]
    private float range;

    [SerializeField]
    private Behaviour[] componentsToDisable;

    private bool on = true;

    private GameObject target;

    void Update()
    {
        if (!target)
        {
            target = GameObject.FindGameObjectWithTag(targetTag);
            return;
        }

        if (on)
        {
            if (Vector2.Distance(transform.position, target.transform.position) > range)
            {
                on = false;
                foreach (Behaviour component in componentsToDisable)
                {
                    component.enabled = false;
                }
                SpriteRenderer spr;
                if (TryGetComponent(out spr)) spr.enabled = false;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, target.transform.position) <= range)
            {
                on = true;
                foreach (Behaviour component in componentsToDisable)
                {
                    component.enabled = true;
                }
                SpriteRenderer spr;
                if (TryGetComponent(out spr)) spr.enabled = true;
            }
        }
    }
}
