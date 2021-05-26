using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EtherealSpanwer : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUTNING };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform etherealFuego;
        public Transform etherealTierra;
        public Transform etherealAire;
        public Transform etherealAgua;
        public int count; // countdown to next wave
        public float rate; //spawn rate
    }

    public Wave[] waves;
    private int nextWave = 0;//index of next wave

    public float timeBetweenWaves = 5f;
    public float waveCountdown;

    private float searchCoundown = 1f;

    public Transform[] spawnPoints;

    public GameObject FireAreaTrigger;
    public GameObject WaterAreaTrigger;
    public GameObject EarthAreaTrigger;
    public GameObject AirAreaTrigger;

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
        if (!FireAreaTrigger.GetComponent<AreaTrigger>().canSpawn && !WaterAreaTrigger.GetComponent<AreaTrigger>().canSpawn && !EarthAreaTrigger.GetComponent<AreaTrigger>().canSpawn && !AirAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            state = SpawnState.WAITING;
        }

        if (state == SpawnState.WAITING)
        {

            //checar si siguen vivos algunos enemigos
            if (!EnemyIsAlive())
            {
                //Iniciar otra wave
                WaveCompleted();

                return;
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
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

            if (!FireAreaTrigger.GetComponent<AreaTrigger>().canSpawn && !WaterAreaTrigger.GetComponent<AreaTrigger>().canSpawn && !EarthAreaTrigger.GetComponent<AreaTrigger>().canSpawn && !AirAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
            {
                MatarEnemigos();
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)  //metodos que esperan x segundos antes de enecutarse
    {
        Debug.Log("Spawing wave: " + _wave.name);
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnGargoyle(_wave.etherealFuego);
            SpawnGargoyle(_wave.etherealTierra);
            SpawnGargoyle(_wave.etherealAgua);
            SpawnGargoyle(_wave.etherealAire);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnGargoyle(Transform _ethereal)
    {
        Debug.Log("Spawning imp: " + _ethereal.name);


        if (_ethereal.name.Contains("Fuego") && FireAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            Transform _sp = spawnPoints[0];
            Instantiate(_ethereal, _sp.position, _sp.rotation);
        }

        if (_ethereal.name.Contains("Earth") && EarthAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            Transform _sp2 = spawnPoints[1];
            Instantiate(_ethereal, _sp2.position, _sp2.rotation);
        }

        if (_ethereal.name.Contains("Water") && WaterAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            Transform _sp2 = spawnPoints[2];
            Instantiate(_ethereal, _sp2.position, _sp2.rotation);
        }

        if (_ethereal.name.Contains("Air") && AirAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            Transform _sp2 = spawnPoints[3];
            Instantiate(_ethereal, _sp2.position, _sp2.rotation);
        }

    }

    void MatarEnemigos()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < temp.Length; i++)
        {
            Destroy(temp[i]);
        }
    }
}
