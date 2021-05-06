using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item 
{

    public enum ItemType
    {
        FireSpell_2,
        FireSpell_3,
        FireSpell_4,
        WaterSpell_2,
        WaterSpell_3,
        WaterSpell_4,
        EarthSpell_2,
        EarthSpell_3,
        EarthSpell_4,
        AirSpell_2,
        AirSpell_3,
        AirSpell_4,
        FirePotion_Strong,
        WaterPotion_Strong,
        EarthPotion_Strong,
        AirPotion_Strong,
        FirePotion_Weak,
        WaterPotion_Weak,
        EarthPotion_Weak,
        AirPotion_Weak,
        HealthPotion,
        StaminaPotion,
        Shield
    }

    public static int GetCost(ItemType itemType)
    {
        switch(itemType)
        {
            default:
            case ItemType.FireSpell_2:          return 20;
            case ItemType.FireSpell_3:          return 50;
            case ItemType.FireSpell_4:          return 120;
            case ItemType.WaterSpell_2:         return 20;
            case ItemType.WaterSpell_3:         return 50;
            case ItemType.WaterSpell_4:         return 120;
            case ItemType.EarthSpell_2:         return 20;
            case ItemType.EarthSpell_3:         return 50;
            case ItemType.EarthSpell_4:         return 120;
            case ItemType.AirSpell_2:           return 20;
            case ItemType.AirSpell_3:           return 50;
            case ItemType.AirSpell_4:           return 120;
            case ItemType.FirePotion_Strong:    return 15;
            case ItemType.WaterPotion_Strong:   return 15;
            case ItemType.EarthPotion_Strong:   return 15;
            case ItemType.AirPotion_Strong:     return 15;
            case ItemType.FirePotion_Weak:      return 10;
            case ItemType.WaterPotion_Weak:     return 10;
            case ItemType.EarthPotion_Weak:     return 10;
            case ItemType.AirPotion_Weak:       return 10;
            case ItemType.HealthPotion:         return 15;
            case ItemType.StaminaPotion:        return 15;
            case ItemType.Shield:               return 20;
        }
    }

    public static Sprite GetSprite(ItemType itemType)
    {
        switch (itemType)
        {
            default:
            case ItemType.FireSpell_2:          return GameAssets.i.FireSprite;
            case ItemType.FireSpell_3:          return GameAssets.i.FireSprite;
            case ItemType.FireSpell_4:          return GameAssets.i.FireSprite;
            case ItemType.WaterSpell_2:         return GameAssets.i.WaterSprite;
            case ItemType.WaterSpell_3:         return GameAssets.i.WaterSprite;
            case ItemType.WaterSpell_4:         return GameAssets.i.WaterSprite;
            case ItemType.EarthSpell_2:         return GameAssets.i.EarthSprite;
            case ItemType.EarthSpell_3:         return GameAssets.i.EarthSprite;
            case ItemType.EarthSpell_4:         return GameAssets.i.EarthSprite;
            case ItemType.AirSpell_2:           return GameAssets.i.AirSprite;
            case ItemType.AirSpell_3:           return GameAssets.i.AirSprite;
            case ItemType.AirSpell_4:           return GameAssets.i.AirSprite;
            case ItemType.FirePotion_Strong:    return GameAssets.i.FirePotionSprite_Strong;
            case ItemType.WaterPotion_Strong:   return GameAssets.i.WaterPotionSprite_Strong;
            case ItemType.EarthPotion_Strong:   return GameAssets.i.EarthPotionSprite_Strong;
            case ItemType.AirPotion_Strong:     return GameAssets.i.AirPotionSprite_Strong;
            case ItemType.FirePotion_Weak:      return GameAssets.i.FirePotionSprite_Weak;
            case ItemType.WaterPotion_Weak:     return GameAssets.i.WaterPotionSprite_Weak;
            case ItemType.EarthPotion_Weak:     return GameAssets.i.EarthPotionSprite_Weak;
            case ItemType.AirPotion_Weak:       return GameAssets.i.AirPotionSprite_Weak;
            case ItemType.HealthPotion:         return GameAssets.i.HealthPotionSprite;
            case ItemType.StaminaPotion:        return GameAssets.i.StaminaPotionSprite;
            case ItemType.Shield:               return GameAssets.i.ShieldSprite;
        }
    }
}
