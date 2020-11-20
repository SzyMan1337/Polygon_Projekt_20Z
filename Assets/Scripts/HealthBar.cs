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

        healthComponent.HealthChange += ChangeHealth;
        slider.maxValue = healthComponent.MaxHealth;
        slider.value = healthComponent.MaxHealth;
    }

    private void ChangeHealth()
    {
        slider.value = healthComponent.CurrentHealth;
    }
}