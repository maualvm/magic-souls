using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    {
        get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }
    }

    public Sprite GeneralSoul, FireSoul, WaterSoul, EarthSoul, AirSoul;
    public Sprite FireSprite, WaterSprite, EarthSprite, AirSprite, FirePotionSprite_Strong, WaterPotionSprite_Strong, EarthPotionSprite_Strong, AirPotionSprite_Strong;
    public Sprite FirePotionSprite_Weak, WaterPotionSprite_Weak, EarthPotionSprite_Weak, AirPotionSprite_Weak, HealthPotionSprite, StaminaPotionSprite, ShieldSprite;
}
