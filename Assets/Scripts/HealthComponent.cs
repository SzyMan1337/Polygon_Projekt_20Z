using UnityEngine;
using UnityEngine.Assertions;


public class HealthComponent : MonoBehaviour
{
    [SerializeField, Range(1.0f, 1000.0f)] private float maxHealth = 10.0f;
    private float currentHealth = 1.0f;


    public event System.Action OnHealthChange;
    public event System.Action OnHit;
    public event System.Action OnDeath;


    public bool IsAlive => currentHealth > 0.0f;
    public float MaxHealth => maxHealth;
    public float HealthPercentage => currentHealth / maxHealth;

    public float CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            if (currentHealth != value)
            {
                currentHealth = Mathf.Clamp(value, 0.0f, maxHealth);
                OnHealthChange?.Invoke();
            }
        }
    }


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount)
    {
        Assert.IsTrue(amount > 0.0f);
        if (IsAlive)
        {
            CurrentHealth -= amount;
            if (IsAlive)
            {
                OnHit?.Invoke();
            }
            else
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
        }
    }

    public void ApplyHeal(float amount)
    {
        Assert.IsTrue(amount > 0.0f);
        if (IsAlive)
        {
            CurrentHealth += amount;
        }
    }
}
