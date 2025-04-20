using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpawnerScr : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float cooldown;
    [SerializeField] private float radius;
    [SerializeField] private float force;
    [SerializeField] private float rotationAngle;

    [SerializeField] private float eliteChance;

    private Collider2D[] cachedResults;

    private float hitProgress = 0f;

    private void Start()
    {
        StartCoroutine(spawnCheck());
        cachedResults = new Collider2D[50];
    }

    private void Update()
    {
        if (hitProgress > 0) hitProgress -= Time.deltaTime * 2.5f;
        if (hitProgress < 0) hitProgress = 0;
        GetComponent<SpriteRenderer>().material.SetFloat("_HitProgress", hitProgress);
    }

    IEnumerator spawnCheck()
    { 
        yield return new WaitForSeconds(cooldown + Random.Range(-cooldown/8, cooldown/8));

        int count = Physics2D.OverlapCircleNonAlloc(transform.position, radius, cachedResults);
        int enemyCount = 0;
        for (int i = 0; i < count; i++)
        {
            if (cachedResults[i].gameObject.CompareTag("Enemy"))
            {
                enemyCount++;
            }
        }
        if (enemyCount < maxEnemies)
            SpawnEnemy();
        StartCoroutine(spawnCheck());
    }

    void SpawnEnemy()
    {
        float angle = Random.Range(0, 2 * Mathf.PI);
        Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
        GameObject enemyObj = Instantiate(enemy, (Vector2)transform.position + direction*0.5f, Quaternion.Euler(rotationAngle, 0, 0));
        hitProgress = 1f;
        enemyObj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
        enemyObj.GetComponent<EliteScr>().chance = eliteChance;
        enemyObj.transform.parent = G.Instance.currentWorldObj.transform;
    }
}
