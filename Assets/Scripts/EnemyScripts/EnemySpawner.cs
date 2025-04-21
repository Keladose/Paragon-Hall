using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject[] enemyPrefabs;        
    public int numberToSpawn = 5;
    public Transform[] spawnPoints;

    void Start()
    {
        // Start a coroutine for each spawn point to spawn enemies simultaneously
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            StartCoroutine(SpawnEnemiesAtPoint(spawnPoints[i], i));
        }
    }

    IEnumerator SpawnEnemiesAtPoint(Transform spawnPoint,int spawnIndex)
    {
        int enemiesPerPoint = numberToSpawn/spawnPoints.Length;
        int remainder = numberToSpawn % spawnPoints.Length;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            yield return new WaitForSeconds(Random.Range(0f, 1f));

            // Randomly choose an enemy from the prefab array
            GameObject enemyPrefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Length)];

            // Instantiate the enemy at the chosen spawn point
            Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
            
           
        }
    }
}
