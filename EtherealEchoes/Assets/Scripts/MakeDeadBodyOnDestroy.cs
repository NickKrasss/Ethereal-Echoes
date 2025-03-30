using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeDeadBodyOnDestroy : MonoBehaviour
{
    [SerializeField] private Sprite baseSprite;
    [SerializeField] private int partsCount;
    [SerializeField] private int minSize;
    [SerializeField] private int maxSize;

    [SerializeField] private float fallSpeed;
    [SerializeField] private float spreadForce;
    [SerializeField] private float spreadForceRandomOffset;

    [SerializeField] private float partPosRandomOffset;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        if (baseSprite == null) baseSprite = GetComponent<SpriteRenderer>().sprite;
        GameObject deadBody = new GameObject(gameObject.name + "_deadBody");
        deadBody.transform.position = transform.position;
        deadBody.transform.rotation = transform.rotation;

        for (int i = 0; i < partsCount; i++)
        {
            GameObject part = new GameObject(gameObject.name + "_deadBody_part");
            part.transform.SetParent(deadBody.transform);
            part.transform.position = new Vector3(deadBody.transform.position.x + Random.Range(-partPosRandomOffset, partPosRandomOffset), deadBody.transform.position.y + Random.Range(-partPosRandomOffset, partPosRandomOffset), deadBody.transform.position.z);
            part.transform.rotation = deadBody.transform.rotation;
            int sizex = Random.Range(minSize, maxSize);
            int sizey = Random.Range(minSize, maxSize);
            SpriteRenderer partRender = part.AddComponent<SpriteRenderer>();
            partRender.sprite = SpriteSplitParticlesScr.GenerateRandomSprite(baseSprite, sizex, sizey);
            partRender.sortingOrder = GetComponent<SpriteRenderer>().sortingOrder - 1;
        }


        DeadBodyScr dbs = deadBody.AddComponent<DeadBodyScr>();
        dbs.fallSpeed = fallSpeed;
        dbs.spreadForce = spreadForce;
        dbs.spreadForceRandomOffset = spreadForceRandomOffset;
    }
}
