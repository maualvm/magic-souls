﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BerserkerSpanwer : MonoBehaviour
{
    public enum SpawnState { SPAWNING, WAITING, COUTNING };
    [System.Serializable]
    public class Wave
    {
        public string name;
        public Transform berserkerFuego;
        public Transform berserkerTierra;
        public Transform berserkerAire;
        public Transform berserkerAgua;
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

    private void OnEnable()
    {
        Player.PlayerWon += HandleWin;
        AreaTrigger.EnteredArea += KillOtherEnemies;
    }

    private void OnDisable()
    {
        Player.PlayerWon -= HandleWin;
        AreaTrigger.EnteredArea -= KillOtherEnemies;
    }

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
        state = SpawnState.SPAWNING;

        for (int i = 0; i < _wave.count; i++)
        {
            SpawnBerserker(_wave.berserkerFuego);
            SpawnBerserker(_wave.berserkerTierra);
            SpawnBerserker(_wave.berserkerAgua);
            SpawnBerserker(_wave.berserkerAire);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnBerserker(Transform _berserker)
    {


        if (_berserker.name.Contains("Fuego") && FireAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            Transform _sp = spawnPoints[Random.Range(0, 3)];
            Instantiate(_berserker, _sp.position, _sp.rotation);
        }

        if (_berserker.name.Contains("Earth") && EarthAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            Transform _sp2 = spawnPoints[Random.Range(3, 6)];
            Instantiate(_berserker, _sp2.position, _sp2.rotation);
        }

        if (_berserker.name.Contains("Water") && WaterAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            Transform _sp2 = spawnPoints[Random.Range(6, 9)];
            Instantiate(_berserker, _sp2.position, _sp2.rotation);
        }

        if (_berserker.name.Contains("Air") && AirAreaTrigger.GetComponent<AreaTrigger>().canSpawn)
        {
            Transform _sp2 = spawnPoints[Random.Range(9, 11)];
            Instantiate(_berserker, _sp2.position, _sp2.rotation);
        }

    }

    private void KillOtherEnemies(string area)
    {
        MatarEnemigos();
    }

    void MatarEnemigos()
    {
        GameObject[] temp = GameObject.FindGameObjectsWithTag("Enemy");

        for (int i = 0; i < temp.Length; i++)
        {
            Destroy(temp[i]);
        }
    }

    private void HandleWin()
    {
        MatarEnemigos();
        enabled = false;
    }
}
