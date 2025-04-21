using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingleSpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;
    public Transform spawnPoint;
    public float minSpawnDelay = 0f;
    public float maxSpawnDelay = 3f;
    public int totalEnemiesToSpawn = 10;

    private int spawnedCount = 0;

    void Start()
    {
        StartCoroutine(SpawnEnemiesOneByOne());
    }

    IEnumerator SpawnEnemiesOneByOne()
    {
        while (spawnedCount < totalEnemiesToSpawn)
        {
            float delay = Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            GameObject prefabToSpawn = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];
            Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

            spawnedCount++;
        }
    }
}

