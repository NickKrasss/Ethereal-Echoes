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
        Instantiate(deadBody, transform.position, transform.rotation);
    }
}
