using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    List<Interactable> interactables = new List<Interactable>();
    private Collider2D[] cachedResults;
    [SerializeField] private float interactionRadius;
    Interactable currentClosest;
    void Start()
    {
        
    }

    void Update()
    {
        RefreshInteractables();
        UpdateClosestInteractable();
        if (Input.GetKeyDown(KeyCode.E) && currentClosest != null)
        {
            currentClosest.Interact(gameObject);
        }
    }

    private void Awake()
    {
        cachedResults = new Collider2D[15];
    }

    private void RefreshInteractables()
    {
        interactables.Clear();
        int count = Physics2D.OverlapCircleNonAlloc(transform.position,interactionRadius,cachedResults);
        for (int i = 0; i < count; i++)
        {
            if (cachedResults[i].TryGetComponent(out Interactable interactable))
            {
                interactables.Add(interactable);
            }
        }
    }
    private void UpdateClosestInteractable()
    {
        Interactable closest = null;
        float minSqrDistance = float.MaxValue;
        Vector3 position = transform.position;
        foreach (var interactable in interactables)
        {
           float sqrDistance = (interactable.GetGameObject().transform.position - position).sqrMagnitude;
            if (sqrDistance < minSqrDistance)
            {
                minSqrDistance = sqrDistance;
                closest = interactable;
            }
        }
        if (currentClosest != closest)
        {
            currentClosest?.SetHighlight(false);
            closest?.SetHighlight(true);
            currentClosest = closest;
        }
    }
}
