using PrimeTween;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OnHealthChanged : MonoBehaviour
{
    TMP_Text textComponent;
    Slider sliderComponent;

    private void Start()
    {
        textComponent = GetComponent<TMP_Text>();
        sliderComponent = GetComponent<Slider>();

        if (PlayerStats.Instance != null)
        {
            HandleHealthChanged(PlayerStats.Instance.currentHealth, PlayerStats.Instance.maxHealth);
        }

        PlayerStats.Instance.OnHealthChangedEvent += HandleHealthChanged;
    }

    private void OnDestroy()
    {
        if (PlayerStats.Instance != null)
        {
            PlayerStats.Instance.OnHealthChangedEvent -= HandleHealthChanged;
        }
    }

    private void HandleHealthChanged(float newHealth, float maxHealth)
    {
        if (textComponent != null) {
            textComponent.text = $"{newHealth}/{maxHealth}";
        }
        if (sliderComponent != null) {
            sliderComponent.maxValue = maxHealth;
            Tween.Custom(sliderComponent.value, newHealth, 0.5f, value => sliderComponent.value = value);
        }
    }
}