using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class Heal : MonoBehaviour
{
    public HealthComponent healthComponent = null;

    private void Awake()
    {
        healthComponent = GetComponent<HealthComponent>();
        Assert.IsNotNull(healthComponent);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            healthComponent.ApplyHeal(2.0f);
        }

    }
}
