using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SpriteSplitParticlesScr))]
public class SpriteParticlesOnDestroy : MonoBehaviour
{
    private SpriteSplitParticlesScr _spriteSplitParticlesScr;

    private void Awake()
    {
        _spriteSplitParticlesScr = GetComponent<SpriteSplitParticlesScr>();
    }

    private void OnDestroy()
    {
        if (!gameObject.scene.isLoaded) return;
        _spriteSplitParticlesScr.CreateParticles(transform.position);
    }
}
