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
    }

    private void OnDisable()
    {
        Enemy.EnemyKilled -= AddSouls;
    }

    private void AddSouls(string element)
    {
        Debug.Log("Obtained one " + element + " soul.");
        TotalSouls++;
        if(element == "Fire")
        {
            FireSouls++;
        }
        else if (element == "Water")
        {
            WaterSouls++;
        }
        else if (element == "Earth")
        {
            EarthSouls++;
        }
        else if (element == "Air")
        {
            AirSouls++;
        }
    }

    private void ResetSouls()
    {
        TotalSouls = 0;
        FireSouls = 0;
        WaterSouls = 0;
        EarthSouls = 0;
        AirSouls = 0;
    }
}
