using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class DropPickUp : MonoBehaviour
{
    [SerializeField]
    public Drop[] drops;

    [HideInInspector]
    public int mult = 1;

    public void OnDestroy()
    {
        if (gameObject.scene.isLoaded && !G.Instance.isWorldLoading) 
        {
            foreach (Drop drop in drops)
                RandomGears(drop);
        }
    }

    public void RandomGears(Drop drop)
    {
        if (G.Instance.isWorldLoading) return;
        if (Random.Range(0f, 1f) > drop.chance) return;

        int count = Random.Range(drop.minCount, drop.maxCount + 1) * mult;
        for (int i = 0; i < count; i++)
        {
            GameObject obj = Instantiate(drop.obj, transform.position, Quaternion.identity);
            obj.transform.SetParent(G.Instance.currentWorldObj.transform);
            PickUp pickUp = obj.GetComponent<PickUp>();
            if (pickUp != null)
            {
                pickUp.count = Random.Range(drop.minContent, drop.maxContent + 1) + G.Instance.currentLevel - 1;
                float scaleFactor = 0.2f + (pickUp.count - 1) * drop.scaleFactor;
                obj.transform.localScale = new Vector3(scaleFactor, scaleFactor, 1f);
            }
            Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                float angle = Random.Range(0, 2 * Mathf.PI); 
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)); 
                rb.AddForce(direction * drop.dropForce, ForceMode2D.Impulse);
            }
        }
    }

    [System.Serializable]
    public class Drop
    {
        [SerializeField]
        public GameObject obj;
        [SerializeField]
        public int minCount = 1;
        [SerializeField]
        public int maxCount = 3;
        [SerializeField]
        public int minContent = 1;
        [SerializeField]
        public int maxContent = 3;
        [SerializeField]
        public float dropForce = 0.5f;

        [SerializeField]
        public float scaleFactor = 0.04f;

        [SerializeField]
        public float chance = 1;
    }
} 
