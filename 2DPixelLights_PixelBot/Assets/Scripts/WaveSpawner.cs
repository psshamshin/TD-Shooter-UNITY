using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour
{
    public enum SpawnState {SPAWN, WAIT, COUNT};
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    
    public Wave[] waves;
    private int nextWave = 0;
    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float SearchCountdown = 1f;
    public Transform[] spawnPoints;

    private SpawnState state = SpawnState.COUNT;

    void Start()
    {
        waveCountdown = timeBetweenWaves;

    }

    // Update is called once per frame
    void Update()
    {
        if (state == SpawnState.WAIT)
        {
            if (!EnemyIsAlive())
            {
                
                WaveCompleted();
            }
            else
            {
                return;
            }
        }
        if (waveCountdown <= 0)
        {
            if(state != SpawnState.SPAWN)
            {
                StartCoroutine( SpawnWave (waves [nextWave] ) );
            }
        } else
        {
            waveCountdown -= Time.deltaTime;
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave completed");
        state = SpawnState.COUNT;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
        }
        else
        {
            nextWave++;
        }
        
    }

    bool EnemyIsAlive()
    {
        SearchCountdown -= Time.deltaTime;
        if (SearchCountdown <= 0f)
        {
            SearchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
  
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        Debug.Log("Spawning" + _wave.name);
        state = SpawnState.SPAWN;
        for (int i = 0; i < _wave.count; i++)
        {
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1f / _wave.rate);
        }
        state = SpawnState.WAIT;

        yield break;
    }

    void SpawnEnemy(Transform _enemy)
    {
        Debug.Log("enemy is spawned" + _enemy.name);
        Transform randomSP = spawnPoints[ Random.Range(0, spawnPoints.Length) ];
        Instantiate(_enemy, randomSP.position, randomSP.rotation);
        

    }
}
