using UnityEngine;
using UnityEngine.Assertions;


public class HealthComponent : MonoBehaviour
{
    [SerializeField, Range(1.0f, 1000.0f)] private float maxHealth = 10.0f;
    private float currentHealth = 1.0f;


    public event System.Action OnDeath;
    public event System.Action OnHit;


    public bool IsAlive => currentHealth > 0.0f;
    public float CurrentHealth => currentHealth;
    public float MaxHealth => maxHealth;
    public float HealthPercentage => currentHealth / maxHealth;


    private void Awake()
    {
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount)
    {
        Assert.IsTrue(amount > 0.0f);
        if (IsAlive)
        {
            currentHealth = Mathf.Max(currentHealth - amount, 0.0f);
            if (currentHealth <= 0.0f)
            {
                OnDeath?.Invoke();
                Destroy(gameObject);
            }
            else
            {
                OnHit?.Invoke();
            }
        }
    }

    public void ApplyHeal(float amount)
    {
        Assert.IsTrue(amount > 0.0f);
        if (IsAlive)
        {
            currentHealth = Mathf.Min(currentHealth + amount, maxHealth);
        }
    }
}
