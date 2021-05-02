using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    [SerializeField]
    private int TotalSouls;
    [SerializeField]
    private int FireSouls;
    [SerializeField]
    private int WaterSouls;
    [SerializeField]
    private int EarthSouls;
    [SerializeField]
    private int AirSouls;

    public static event Action<int> ModifiedSouls, ModifiedFireSouls, ModifiedWaterSouls, ModifiedEarthSouls, ModifiedAirSouls;
    // Start is called before the first frame update
    void Start()
    {
        ResetSouls();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        Enemy.EnemyKilled += AddSouls;
        Player.PlayerKilled += ResetSouls;
    }

    private void OnDisable()
    {
        Enemy.EnemyKilled -= AddSouls;
        Player.PlayerKilled -= ResetSouls;
    }

    private void AddSouls(string element)
    {
        Debug.Log("Obtained one " + element + " soul.");
        TotalSouls++;
        ModifiedSouls?.Invoke(TotalSouls);
        if (element == "Fire")
        {
            FireSouls++;
            ModifiedFireSouls?.Invoke(FireSouls);
        }
        else if (element == "Water")
        {
            WaterSouls++;
            ModifiedWaterSouls?.Invoke(WaterSouls);
        }
        else if (element == "Earth")
        {
            EarthSouls++;
            ModifiedEarthSouls?.Invoke(EarthSouls);
        }
        else if (element == "Air")
        {
            AirSouls++;
            ModifiedAirSouls?.Invoke(AirSouls);
        }
    }

    private void ResetSouls()
    {
        TotalSouls = 0;
        FireSouls = 0;
        WaterSouls = 0;
        EarthSouls = 0;
        AirSouls = 0;
        ModifiedSouls?.Invoke(TotalSouls);
        ModifiedFireSouls?.Invoke(TotalSouls);
        ModifiedWaterSouls?.Invoke(TotalSouls);
        ModifiedEarthSouls?.Invoke(TotalSouls);
        ModifiedAirSouls?.Invoke(TotalSouls);
    }
}
