using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Damage : MonoBehaviour
{
    public HealthComponent healthComponent = null;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        Assert.IsNotNull(healthComponent);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            healthComponent.ApplyDamage(2.0f);
        }

    }
}
