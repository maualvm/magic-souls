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

    [SerializeField]
    public int HealthPotions;

    [SerializeField]
    public int StaminaPotions;

    public static event Action<int> ModifiedSouls, ModifiedFireSouls, ModifiedWaterSouls, ModifiedEarthSouls, ModifiedAirSouls, ModifiedHealthPotions, ModifiedStaminaPotions;
    public static event Action<Item.ItemType> BoughtItem;
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
        Enemy.EnemyKilled += ModifySouls;
        Player.PlayerKilled += ResetSouls;
        ShopSystem.TryBuyItem += TryToBuyItem;
        ShopSystem.HealthPotionBought += ModifyHealthPotions;
        ShopSystem.StaminaPotionBought += ModifyStaminaPotions;
    }

    private void OnDisable()
    {
        Enemy.EnemyKilled -= ModifySouls;
        Player.PlayerKilled -= ResetSouls;
        ShopSystem.TryBuyItem -= TryToBuyItem;
        ShopSystem.HealthPotionBought -= ModifyHealthPotions;
        ShopSystem.StaminaPotionBought -= ModifyStaminaPotions;
    }

    private void ModifySouls(string element, int amount)
    {
        Debug.Log("Obtained one " + element + " soul.");
        AudioManager.PlaySound(AudioManager.Sound.GetSoul);
        TotalSouls += amount;
        ModifiedSouls?.Invoke(TotalSouls);
        if (element == "Fire")
        {
            FireSouls += amount;
            ModifiedFireSouls?.Invoke(FireSouls);
        }
        else if (element == "Water")
        {
            WaterSouls += amount;
            ModifiedWaterSouls?.Invoke(WaterSouls);
        }
        else if (element == "Earth")
        {
            EarthSouls += amount;
            ModifiedEarthSouls?.Invoke(EarthSouls);
        }
        else if (element == "Air")
        {
            AirSouls += amount;
            ModifiedAirSouls?.Invoke(AirSouls);
        }
    }

    public void ModifyHealthPotions(int amount)
    {
        if(HealthPotions <= 0) {
            HealthPotions = 0;
        }
        HealthPotions += amount;
        ModifiedHealthPotions?.Invoke(HealthPotions);
    }

    public void ModifyStaminaPotions(int amount)
    {
        StaminaPotions += amount;
        ModifiedStaminaPotions?.Invoke(StaminaPotions);
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

    private void TryToBuyItem(Item.ItemType itemType, int itemCost, string itemElement)
    {
        if (TrySpendSouls(itemCost, itemElement))
        {
            AudioManager.PlaySound(AudioManager.Sound.Buy);
            BoughtItem?.Invoke(itemType);
        }
        else
        {
            AudioManager.PlaySound(AudioManager.Sound.CantBuy);
            TooltipWarningScreenSpaceUI.ShowTooltip_Static("You don't have " + itemCost + " " + itemElement + " Souls!", 1);
        }
    }

    public bool TrySpendSouls(int soulsAmount, string elementType)
    {
        if (elementType == "General" && TotalSouls >= soulsAmount)
        {
            TotalSouls -= soulsAmount;
            ModifiedSouls?.Invoke(TotalSouls);
            return true;
        }
        else if (elementType == "Fire" && FireSouls >= soulsAmount)
        {
            FireSouls -= soulsAmount;
            ModifiedFireSouls?.Invoke(FireSouls);
            return true;
        }
        else if (elementType == "Water" && WaterSouls >= soulsAmount)
        {
            WaterSouls -= soulsAmount;
            ModifiedWaterSouls?.Invoke(WaterSouls);
            return true;
        }
        else if (elementType == "Earth" && EarthSouls >= soulsAmount)
        {
            EarthSouls -= soulsAmount;
            ModifiedEarthSouls?.Invoke(EarthSouls);
            return true;
        }
        else if (elementType == "Air" && AirSouls >= soulsAmount)
        {
            AirSouls -= soulsAmount;
            ModifiedAirSouls?.Invoke(AirSouls);
            return true;
        }
        else
            return false;
    }
}
