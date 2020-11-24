using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    private HealthComponent healthComponent;


    private void Awake()
    {
        Assert.IsNotNull(slider);
        healthComponent = GetComponentInParent<HealthComponent>();
        Assert.IsNotNull(healthComponent);

        healthComponent.OnHealthChange += OnChangeHealth;
        slider.maxValue = healthComponent.MaxHealth;
        slider.value = healthComponent.MaxHealth;
    }

    private void OnChangeHealth()
    {
        slider.value = healthComponent.CurrentHealth;
    }
}