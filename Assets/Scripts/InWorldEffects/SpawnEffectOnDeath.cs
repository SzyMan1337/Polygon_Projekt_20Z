using UnityEngine;
using UnityEngine.Assertions;


public class SpawnEffectOnDeath : MonoBehaviour
{
    [SerializeField] private GameObject effectToSpawnPrefab = null;
    private HealthComponent healthComponent = null;


    private void Awake()
    {
        Assert.IsNotNull(effectToSpawnPrefab);
        healthComponent = GetComponent<HealthComponent>();
        Assert.IsNotNull(healthComponent);
        healthComponent.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        Instantiate(effectToSpawnPrefab, transform.position, transform.rotation);
    }
}
