using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;
    public float respawnDelay = 5f;

    private GameObject currentEnemy;

    void Start()
    {
        SpawnEnemy();
    }

    void SpawnEnemy()
    {
        currentEnemy = Instantiate(enemyPrefab, transform.position, Quaternion.identity);
        StartCoroutine(WaitForEnemyDeath());
    }

    IEnumerator WaitForEnemyDeath()
    {
        while (currentEnemy != null)
        {
            yield return null;
        }

        yield return new WaitForSeconds(respawnDelay);

        SpawnEnemy();
    }
}
