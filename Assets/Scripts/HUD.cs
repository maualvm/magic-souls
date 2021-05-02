using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    Image SpellSelectionWheel;

    [SerializeField]
    Sprite WaterSpellSelected;

    [SerializeField]
    Sprite FireSpellSelected;

    [SerializeField]
    Sprite AirSpellSelected;

    [SerializeField]
    Sprite EarthSpellSelected;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            SpellSelectionWheel.sprite = WaterSpellSelected;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SpellSelectionWheel.sprite = FireSpellSelected;
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SpellSelectionWheel.sprite = AirSpellSelected;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SpellSelectionWheel.sprite = EarthSpellSelected;
        }
    }

    private void OnEnable()
    {
        Player.PlayerDamaged += HandleHealthChange;
        Player.StaminaChanged += HandleStaminaChange;
        InventorySystem.IncreasedSouls += HandleGeneralSoulsChange;
        InventorySystem.IncreasedFireSouls += HandleFireSoulsChange;
        InventorySystem.IncreasedWaterSouls += HandleWaterSoulsChange;
        InventorySystem.IncreasedEarthSouls += HandleEarthSoulsChange;
        InventorySystem.IncreasedAirSouls += HandleAirSoulsChange;
    }

    private void OnDisable()
    {
        Player.PlayerDamaged -= HandleHealthChange;
        Player.StaminaChanged -= HandleStaminaChange;
        InventorySystem.IncreasedSouls += HandleGeneralSoulsChange;
        InventorySystem.IncreasedFireSouls += HandleFireSoulsChange;
        InventorySystem.IncreasedWaterSouls += HandleWaterSoulsChange;
        InventorySystem.IncreasedEarthSouls += HandleEarthSoulsChange;
        InventorySystem.IncreasedAirSouls += HandleAirSoulsChange;
    }

    private void HandleHealthChange(float newHealth, float maxHealth)
    {
        StartCoroutine(ChangeHealth(newHealth, maxHealth));
    }    

    private IEnumerator ChangeHealth(float newHealth, float maxHealth)
    {
        HealthText.text = newHealth + "/" + maxHealth;

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
}
