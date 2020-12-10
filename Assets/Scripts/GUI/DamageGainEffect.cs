using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class DamageGainEffect : MonoBehaviour
{
    private const float DAMAGE_EFFECT_IMAGE_MAX_ALPHA = 0.6f;
    private const float DAMAGE_EFFECT_IMAGE_ALPHA_CHANGE_SPEED = 0.5f;
    private Image image;
    private HealthComponent healthComponent;


    private void Awake()
    {
        image = GetComponent<Image>();
        Assert.IsNotNull(image);
        healthComponent = GetComponentInParent<HealthComponent>();
        Assert.IsNotNull(healthComponent);

        healthComponent.OnHealthChange += DamageEffect;
    }

    private void Update()
    {
        if(image.color.a > 0.0f)
        {
            image.color = new Color(image.color.r, image.color.g, image.color.b, image.color.a - (DAMAGE_EFFECT_IMAGE_ALPHA_CHANGE_SPEED * Time.deltaTime));
        }
    }

    private void DamageEffect()
    {
        image.color = new Color(image.color.r, image.color.g, image.color.b, DAMAGE_EFFECT_IMAGE_MAX_ALPHA);
    }
}
