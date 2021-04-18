using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImpSpawner : MonoBehaviour
{
    public enum SpawnState {SPAWNING, WAITING, COUTNING};
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform impFuego;
        public int count; // countdown to next wave
        public float rate; //spawn rate
    }

    public Wave[] waves;
    private int nextWave = 0;//index of next wave
    
    public float timeBetweenWaves = 5f;
    private float waveCountdown;

    private float searchCoundown = 1f;

    public Transform[] spawnPoints;

    private SpawnState state = SpawnState.COUTNING;

    void Start()
    {

        if (spawnPoints.Length == 0)
        {
            Debug.LogError("No spawn points for this spawner" + name);
        }

        waveCountdown = timeBetweenWaves;

    }

    void Update()
    {
 
        if (state == SpawnState.WAITING)
        {
           
            //checar si siguen vivos algunos enemigos
            if (!EnemyIsAlive())
            {
                //Iniciar otra wave
                WaveCompleted();

                return;
            } else
            {
                return;
            }
        }

        if ( waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }

        }
        else
        {
            //rebajar el countdown
            waveCountdown -= Time.deltaTime;

        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave completed!");
        state = SpawnState.COUTNING;
        waveCountdown = timeBetweenWaves;
    }

    bool EnemyIsAlive()
    {
        searchCoundown -= Time.deltaTime;
        if (searchCoundown <= 0)
        {
            searchCoundown = 1f;
            if (GameObject.FindGameObjectsWithTag("Enemy").Length == 0)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)  //metodos que esperan x segundos antes de enecutarse
    {
        Debug.Log("Spawing wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for(int i = 0; i < _wave.count; i++)
        {
            SpawnImp(_wave.impFuego);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnImp(Transform _imp)
    {
        Debug.Log("Spawning imp: " + _imp.name);
       
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_imp, _sp.position, _sp.rotation);
    }
}
