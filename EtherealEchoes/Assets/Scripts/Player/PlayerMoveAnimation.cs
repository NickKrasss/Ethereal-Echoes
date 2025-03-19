using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WASDMovementScr))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(FlipSpriteToMouse))]
public class PlayerMoveAnimation : MonoBehaviour
{
    private Animator animator;
    private WASDMovementScr wasdMovement;
    private FlipSpriteToMouse flipScr;
    void Start()
    {
        animator = GetComponent<Animator>();
        wasdMovement = GetComponent<WASDMovementScr>();
        flipScr = GetComponent<FlipSpriteToMouse>();
    }

    void Update()
    {
        if (Time.timeScale == 0f) return;
        animator.SetFloat("MoveBlend", Mathf.Lerp(animator.GetFloat("MoveBlend"), wasdMovement.wasdVector != Vector2.zero ? 1 : 0, 10 * Time.deltaTime));
        animator.SetFloat("MoveSpeedMult", Mathf.Sqrt(wasdMovement.speed / 4));
        animator.SetBool("FaceRight", !flipScr.isFlipped);
    }
}
