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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnEnable()
    {
        Player.PlayerDamaged += HandleHealthChange;
        Player.StaminaChanged += HandleStaminaChange;
    }

    private void OnDisable()
    {
        Player.PlayerDamaged -= HandleHealthChange;
        Player.StaminaChanged -= HandleStaminaChange;
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
}
