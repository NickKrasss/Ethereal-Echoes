using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipSpriteToMouse : MonoBehaviour
{

    public bool isFlipped;

    void Update()
    {
        if (Time.timeScale != 0f)
        {
            isFlipped = WorldMousePosition.GetWorldMousePosition(Camera.main).x < transform.position.x;
            transform.localScale = new Vector3(isFlipped ? -1 : 1, transform.localScale.y, 1);
        }
    }
}
