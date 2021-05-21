using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    [SerializeField]
    private Image Foreground;

    [SerializeField]
    float UpdateSpeed;

    private void OnEnable()
    {
        Enemy.EnemyHealthChanged += HandleHealthChange;
    }

    private void OnDisable()
    {
        Enemy.EnemyHealthChanged -= HandleHealthChange;
    }

    private void HandleHealthChange(float newHealth, float maxHealth)
    {
        StartCoroutine(ChangeHealth(newHealth, maxHealth));
    }

    private IEnumerator ChangeHealth(float newHealth, float maxHealth)
    {

        float PreviousHealth = Foreground.fillAmount;
        float TimeElapsed = 0f;

        while (TimeElapsed < UpdateSpeed)
        {
            TimeElapsed += Time.deltaTime;
            Foreground.fillAmount = Mathf.Lerp(PreviousHealth, newHealth / maxHealth, TimeElapsed / UpdateSpeed);
            yield return null;
        }

        Foreground.fillAmount = newHealth / maxHealth;
    }

    private void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        transform.Rotate(0, 180, 0);
    }
}
