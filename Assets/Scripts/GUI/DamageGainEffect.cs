using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class DamageGainEffect : MonoBehaviour
{
    private Image image;
    private HealthComponent healthComponent;

    private void Awake()
    {
        image = GetComponent<Image>();
        Assert.IsNotNull(image);
        healthComponent = GetComponentInParent<HealthComponent>();
        Assert.IsNotNull(healthComponent);

        healthComponent.OnHealthChange += RedEffect;
    }

    private void Update()
    {
        if(image.color.a > 0)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - 0.01f);
        }
    }

    private void RedEffect()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, 0.6f);
    }
}
