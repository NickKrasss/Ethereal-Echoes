using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

[RequireComponent(typeof(SpriteSkin))]
public class SpawnPartsOnDeath : MonoBehaviour
{
    [SerializeField]
    private GameObject deadBody;

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        if (G.Instance.isWorldLoading) return;
        DeadBodyScr dbs = Instantiate(deadBody, transform.position, transform.rotation).GetComponent<DeadBodyScr>();
        dbs.transform.SetParent(G.Instance.currentWorldObj.transform);
        dbs.flip = GetComponent<SpriteRenderer>().flipX;
    }
}
