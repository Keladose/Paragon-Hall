using Spellect;
using System;
using System.Collections;
using UnityEngine;

public class SingleSpawner : Spawner
{
    public GameObject[] enemyPrefabs;
    public Transform spawnPoint;
    public float minSpawnDelay = 0f;
    public float maxSpawnDelay = 3f;
    public int totalEnemiesToSpawn = 10;

    private int spawnedCount = 0;

    void Start()
    {
        
    }

    IEnumerator SpawnEnemiesOneByOne()
    {
        while (spawnedCount < totalEnemiesToSpawn)
        {
            float delay = UnityEngine.Random.Range(minSpawnDelay, maxSpawnDelay);
            yield return new WaitForSeconds(delay);

            GameObject prefabToSpawn = currentWaveEnemyPrefabs[UnityEngine.Random.Range(0, currentWaveEnemyPrefabs.Count)];
            GameObject newEnemy = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);

            OnKillAllEnemies += newEnemy.GetComponent<HealthController>().Kill;
            CaptureEnemyDeath(newEnemy);
            OnEnemySpawned?.Invoke(this, new EventArgs());
            spawnedCount++;
        }
    }


    public override void SpawnNextWave()
    {
        _nextWave++;
        currentWaveEnemyPrefabs.Clear();
        foreach (int i in wavePrefabs[_nextWave].innerList)
        {
            currentWaveEnemyPrefabs.Add(enemyPrefabs[i]);
        }
        StartCoroutine(SpawnEnemiesOneByOne());
    }

}

