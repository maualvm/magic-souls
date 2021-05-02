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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(HealthImage.fillAmount);
    }

    private void OnEnable()
    {
        Player.PlayerDamaged += HandleHealthChange;
    }

    private void OnDisable()
    {
        Player.PlayerDamaged -= HandleHealthChange;   
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
}
