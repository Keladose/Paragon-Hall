using Spellect;
using System;
using System.Collections;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public GameObject[] enemyPrefabs;      
    public Transform[] spawnPoints;



    void Start()
    {

            // Start a coroutine for each spawn point to spawn enemies simultaneously
        
    }

    public override void SpawnNextWave()
    {
        if (_isDisabled)
        {
            return;
        }
        _nextWave++;
        currentWaveEnemyPrefabs.Clear();
        foreach (int i in wavePrefabs[_nextWave].innerList)
        {
            currentWaveEnemyPrefabs.Add(enemyPrefabs[i]);
        }
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            StartCoroutine(SpawnEnemiesAtPoint(spawnPoints[i], i));
        }
    }

    IEnumerator SpawnEnemiesAtPoint(Transform spawnPoint,int spawnIndex)
    {
        int enemiesPerPoint = enemiesPerWave[_nextWave] / spawnPoints.Length;
        int remainder = enemiesPerWave[_nextWave] % spawnPoints.Length;

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(0f, 1f));

            // Randomly choose an enemy from the prefab array
            GameObject enemyPrefab = currentWaveEnemyPrefabs[UnityEngine.Random.Range(0, currentWaveEnemyPrefabs.Count)];

            // Instantiate the enemy at the chosen spawn point
            GameObject newEnemy = Instantiate(enemyPrefab, spawnPoints[i].position, Quaternion.identity);
            OnKillAllEnemies += newEnemy.GetComponent<HealthController>().Kill;
            CaptureEnemyDeath(newEnemy);

            OnEnemySpawned?.Invoke(this, new EventArgs());
        }
    }

}
