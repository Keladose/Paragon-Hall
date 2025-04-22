using Spellect;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour
{

    public delegate void OnEnemyChangedEvent(object source, EventArgs e);
    public OnEnemyChangedEvent OnEnemySpawned;

    public OnEnemyChangedEvent OnEnemyDied;
    public delegate void OnKillAllEnemiesEvent(object source, EventArgs e);
    public OnKillAllEnemiesEvent OnKillAllEnemies;
    protected bool _isDisabled = false;

    protected List<GameObject> currentWaveEnemyPrefabs = new();
    public List<IntListWrapper> wavePrefabs = new();
    public List<int> enemiesPerWave = new();
    protected int _nextWave = -1;
    public int numWaves = 1;
    public void Disable()
    {
        _isDisabled = true;
    }

    protected void OnEnemyDeath(object o, EventArgs e)
    {
        OnEnemyDied?.Invoke(this, new EventArgs());
    }

    protected void CaptureEnemyDeath(GameObject enemy)
    {
        enemy.GetComponent<HealthController>().OnDeath += OnEnemyDeath;
    }

    public abstract void SpawnNextWave();

    public void KillAllEnemies()
    {
        OnKillAllEnemies?.Invoke(this, new EventArgs());
    }
}
[System.Serializable]
public class IntListWrapper
{
    public List<int> innerList;
}