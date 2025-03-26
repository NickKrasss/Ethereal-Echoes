using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScr : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private int maxEnemies;
    [SerializeField] private float cooldown;
    [SerializeField] private float radius;
    [SerializeField] private float force;
    [SerializeField] private float rotationAngle;

    private Collider2D[] cachedResults;

    private void Start()
    {
        StartCoroutine(spawnCheck());
        cachedResults = new Collider2D[50];
    }

    IEnumerator spawnCheck()
    { 
        yield return new WaitForSeconds(cooldown);

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
        
        enemyObj.GetComponent<Rigidbody2D>().AddForce(direction * force, ForceMode2D.Impulse);
    }
}
