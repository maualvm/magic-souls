﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class HUD : MonoBehaviour
{

    [SerializeField]
    Image HealthImage;

    [SerializeField]
    TMP_Text HealthText;

    [SerializeField]
    float HealthUpdateSpeed;

    [SerializeField]
    Image StaminaImage;

    [SerializeField]
    TMP_Text StaminaText;

    [SerializeField]
    float StaminaUpdateSpeed;

    [SerializeField]
    TMP_Text GeneralSoulsCount;

    [SerializeField]
    TMP_Text FireSoulsCount;

    [SerializeField]
    TMP_Text WaterSoulsCount;

    [SerializeField]
    TMP_Text EarthSoulsCount;

    [SerializeField]
    TMP_Text AirSoulsCount;

    [SerializeField]
    TMP_Text HealthPotionsCount;

    [SerializeField]
    TMP_Text StaminaPotionsCount;

    

    [SerializeField]
    Sprite WaterSpellSelected;

    [SerializeField]
    Sprite FireSpellSelected;

    [SerializeField]
    Sprite AirSpellSelected;

    [SerializeField]
    Sprite EarthSpellSelected;

    [SerializeField]
    Button RespawnBtn;

    [Header("Screens")]
    [SerializeField]
    GameObject DeathScreen;

    [SerializeField]
    GameObject WinScreen;

    [SerializeField]
    GameObject PauseScreen;


    [Header("Spell Selection")]
    [SerializeField]
    Image SpellSelectionWheel;

    [SerializeField]
    TMP_Text WaterLevelTxt;

    [SerializeField]
    TMP_Text FireLevelTxt;

    [SerializeField]
    TMP_Text EarthLevelTxt;

    [SerializeField]
    TMP_Text AirLevelTxt;

    public static event Action Respawned;
    public static event Action<String> SpellChanged;
    // Start is called before the first frame update
    void Start()
    {
        DeathScreen.SetActive(false);
        WinScreen.SetActive(false);
        PauseScreen.SetActive(false);
        RespawnBtn.onClick.AddListener(() => Respawn());
        SpellSelectionWheel.sprite = WaterSpellSelected;
    }

    // Update is called once per frame
    void Update()
    {
        if(!Player.gameIsPaused)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                SpellSelectionWheel.sprite = WaterSpellSelected;
                SpellChanged?.Invoke("Water");
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                SpellSelectionWheel.sprite = EarthSpellSelected;
                SpellChanged?.Invoke("Earth");
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                SpellSelectionWheel.sprite = AirSpellSelected;
                SpellChanged?.Invoke("Air");
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                SpellSelectionWheel.sprite = FireSpellSelected;
                SpellChanged?.Invoke("Fire");
            }
        }
        
    }

    private void OnEnable()
    {
        Player.PlayerDamaged += HandleHealthChange;
        Player.StaminaChanged += HandleStaminaChange;
        Player.PlayerKilled += HandlePlayerDeath;
        Player.PlayerWon += HandleWin;
        Player.GamePaused += HandleGamePaused;
        InventorySystem.ModifiedSouls += HandleGeneralSoulsChange;
        InventorySystem.ModifiedFireSouls += HandleFireSoulsChange;
        InventorySystem.ModifiedWaterSouls += HandleWaterSoulsChange;
        InventorySystem.ModifiedEarthSouls += HandleEarthSoulsChange;
        InventorySystem.ModifiedAirSouls += HandleAirSoulsChange;
        InventorySystem.ModifiedHealthPotions += HandleHealthPotionChange;
        InventorySystem.ModifiedStaminaPotions += HandleStaminaPotionChange;
        ShopSystem.SpellLevelUp += HandleSpellLevelUp;
    }

    private void OnDisable()
    {
        Player.PlayerDamaged -= HandleHealthChange;
        Player.StaminaChanged -= HandleStaminaChange;
        Player.PlayerKilled -= HandlePlayerDeath;
        Player.PlayerWon -= HandleWin;
        Player.GamePaused -= HandleGamePaused;
        InventorySystem.ModifiedSouls -= HandleGeneralSoulsChange;
        InventorySystem.ModifiedFireSouls -= HandleFireSoulsChange;
        InventorySystem.ModifiedWaterSouls -= HandleWaterSoulsChange;
        InventorySystem.ModifiedEarthSouls -= HandleEarthSoulsChange;
        InventorySystem.ModifiedAirSouls -= HandleAirSoulsChange;
        InventorySystem.ModifiedHealthPotions -= HandleHealthPotionChange;
        InventorySystem.ModifiedStaminaPotions -= HandleStaminaPotionChange;
        ShopSystem.SpellLevelUp -= HandleSpellLevelUp;
    }

    private void HandleGamePaused(bool isPaused)
    {
        if(isPaused)
            PauseScreen.SetActive(true);
        else
            PauseScreen.SetActive(false);
    }

    private void HandleSpellLevelUp(string element, int level)
    {
        if (element == "Fire")
        {
            FireLevelTxt.text = level.ToString();
            if(level == 4)
            {
                FireLevelTxt.color = Color.red;
            }
        }
        else if (element == "Water")
        {
            WaterLevelTxt.text = level.ToString();
            if (level == 4)
            {
                WaterLevelTxt.color = Color.red;
            }
        }
        else if (element == "Earth")
        {
            EarthLevelTxt.text = level.ToString();
            if (level == 4)
            {
                EarthLevelTxt.color = Color.red;
            }
        }
        else if (element == "Air")
        {
            AirLevelTxt.text = level.ToString();
            if (level == 4)
            {
                AirLevelTxt.color = Color.red;
            }
        }


    }

    private void HandleHealthChange(float newHealth, float maxHealth)
    {
        StartCoroutine(ChangeHealth(newHealth, maxHealth));
    }    

    private IEnumerator ChangeHealth(float newHealth, float maxHealth)
    {
        HealthText.text = Mathf.Round(newHealth) + "/" + maxHealth;

        float PreviousHealth = HealthImage.fillAmount;
        float TimeElapsed = 0f;

        while (TimeElapsed < HealthUpdateSpeed)
        {
            TimeElapsed += Time.deltaTime;
            HealthImage.fillAmount = Mathf.Lerp(PreviousHealth, newHealth / 100, TimeElapsed / HealthUpdateSpeed);
            yield return null;
        }

        HealthImage.fillAmount = newHealth / 100;
    }

    private void HandleStaminaChange(float newStamina, float maxStamina)
    {
        StartCoroutine(ChangeStamina(newStamina, maxStamina));
    }

    private IEnumerator ChangeStamina(float newStamina, float maxStamina)
    {
        StaminaText.text = Mathf.Round(newStamina) + "/" + maxStamina;

        float PreviousStamina = StaminaImage.fillAmount;
        float TimeElapsed = 0f;

        while (TimeElapsed < StaminaUpdateSpeed)
        {
            TimeElapsed += Time.deltaTime;
            StaminaImage.fillAmount = Mathf.Lerp(PreviousStamina, newStamina / 100, TimeElapsed / StaminaUpdateSpeed);
            yield return null;
        }

        StaminaImage.fillAmount = newStamina / 100;
    }

    private void HandleGeneralSoulsChange(int count)
    {
        GeneralSoulsCount.text = count.ToString();
    }
    private void HandleFireSoulsChange(int count)
    {
        FireSoulsCount.text = count.ToString();
    }
    private void HandleWaterSoulsChange(int count)
    {
        WaterSoulsCount.text = count.ToString();
    }
    private void HandleEarthSoulsChange(int count)
    {
        EarthSoulsCount.text = count.ToString();
    }
    private void HandleAirSoulsChange(int count)
    {
        AirSoulsCount.text = count.ToString();
    }

    private void HandleHealthPotionChange(int count)
    {
        HealthPotionsCount.text = count.ToString();
    }

    private void HandleStaminaPotionChange(int count)
    {
        StaminaPotionsCount.text = count.ToString();
    }

    private void HandlePlayerDeath()
    {
        DeathScreen.SetActive(true);
    }

    private void Respawn()
    {
        DeathScreen.SetActive(false);
        Respawned?.Invoke();
    }

    private void HandleWin()
    {
        WinScreen.SetActive(true);
    }
    
}
