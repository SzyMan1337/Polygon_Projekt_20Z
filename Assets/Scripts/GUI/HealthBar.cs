using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class HealthBar : MonoBehaviour
{
    private Image image;
    private HealthComponent healthComponent;


    private void Awake()
    {
        image = GetComponent<Image>();
        Assert.IsNotNull(image);
        healthComponent = GetComponentInParent<HealthComponent>();
        Assert.IsNotNull(healthComponent);

        healthComponent.OnHealthChange += ChangeHealth;
    }

    private void Start()
    {
        ChangeHealth();
    }

    private void ChangeHealth()
    {
        image.fillAmount = healthComponent.HealthPercentage;
    }
}