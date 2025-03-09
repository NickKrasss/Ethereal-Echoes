using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(WASDMovementScr))]
[RequireComponent(typeof(Animator))]
public class PlayerMoveAnimation : MonoBehaviour
{
    private Animator animator;
    private WASDMovementScr wasdMovement;
    void Start()
    {
        animator = GetComponent<Animator>();
        wasdMovement = GetComponent<WASDMovementScr>();
    }

    void Update()
    {
        animator.SetFloat("MoveBlend", Mathf.Lerp(animator.GetFloat("MoveBlend"), wasdMovement.wasdVector != Vector2.zero ? 1 : 0, 10 * Time.deltaTime));
        animator.SetFloat("MoveSpeedMult", Mathf.Sqrt(wasdMovement.speed / 4));
    }
}
