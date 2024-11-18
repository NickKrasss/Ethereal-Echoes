using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(HealthScr))]
public class PlayerDeath : MonoBehaviour
{

    [SerializeField]
    private GameObject deathScreen;

    private HealthScr healthScr;

    private bool dead = false;

    void Awake()
    {
        deathScreen = GameObject.FindWithTag("DeathScreen");
        deathScreen.SetActive(false);
        healthScr = GetComponent<HealthScr>(); 
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead && healthScr.health <= 0)
        { 
            dead = true;
            deathScreen.SetActive(true);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<WASDMovementScr>().enabled = false;
            GetComponent<SmoothMoveScr>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        }
        if (dead)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }
}
