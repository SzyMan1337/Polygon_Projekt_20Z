using Unity.Profiling;
using UnityEngine;
using UnityEngine.Assertions;
using System;

public class HealthComponent : MonoBehaviour
{
    private float maxHealth = 10.0f;
    [SerializeField] private float currentHealth = 0.0f;

    public event EventHandler OnDeath;


    public void DeathMessage(object sender, EventArgs e)
    {
        Debug.Log("You have been killed");
    }

    public void Awake()
    {
        OnDeath += DeathMessage;
        currentHealth = maxHealth;
    }

    public void ApplyDamage(float amount)
    {
        Assert.IsTrue(amount > 0.0f);
        if (currentHealth > 0.0f)
        {
            currentHealth -= amount;
            if (currentHealth <= 0.0f)
            {
                OnDeath?.Invoke(this, EventArgs.Empty); // invokes the event only if OnDeath is not null
                Destroy(gameObject);
            }  
        }
    }

    public void ApplyHeal(float amount)
    {
        Assert.IsTrue(amount > 0.0f);
        if (currentHealth + amount <= maxHealth)
        {
            currentHealth += amount;
        }
    }

    public void DisplayHealth()
    {
        Debug.Log("Current health: " +  currentHealth + " Max health: " + maxHealth);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.D))
        {
            DisplayHealth();
        }
    }



}
