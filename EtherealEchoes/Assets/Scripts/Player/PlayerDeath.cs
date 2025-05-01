using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Stats))]
public class PlayerDeath : MonoBehaviour
{

    [SerializeField]
    private GameObject deathScreen;

    [SerializeField]
    private GameObject deadBody;

    private Stats stats;

    private bool dead = false;

    void Awake()
    {
        deathScreen = GameObject.FindWithTag("DeathScreen");
        deathScreen.SetActive(false);
        stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!dead && stats.CurrentHealth <= 0 && PlayerPrefs.GetInt("GodMode") != 1)
        { 
            dead = true;
            G.Instance.playerDead = true;
            DeadBodyScr dbs = Instantiate(deadBody, transform.position, transform.rotation).GetComponent<DeadBodyScr>();
            dbs.flip = GetComponent<SpriteRenderer>().flipX;
            deathScreen.SetActive(true);
            GetComponent<SpriteRenderer>().enabled = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<WASDMovementScr>().enabled = false;
            GetComponent<SmoothMoveScr>().enabled = false;
            GetComponent<PlayerGun>().enabled = false;
            GetComponent<Rigidbody2D>().velocity = Vector3.zero;
            Time.timeScale = 0.4f;
        }
        if (dead)
        {
            if (Input.GetKeyUp(KeyCode.Space))
            {
                TransitionOverlayController.Instance.FadeIn(0.5f, 0f, () => SceneManager.LoadScene(SceneManager.GetActiveScene().name));
                //SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                Time.timeScale = 1f;
            }
        }
    }
}
