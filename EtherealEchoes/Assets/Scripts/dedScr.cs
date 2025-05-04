using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class dedScr : MonoBehaviour
{
    [SerializeField]
    private Sprite deadSpr;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DamageHitbox"))
        {
            GetComponent<Animator>().SetTrigger("dead");
            GetComponent<SpriteRenderer>().sprite = deadSpr;
            transform.position = new Vector3(transform.position.x, transform.position.y, -0.1f);
            InfoPopUpScreenController.Instance.Show("Работа завершена.", 15f);
        }
    }
}
