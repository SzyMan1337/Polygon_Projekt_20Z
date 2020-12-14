using UnityEngine;
using UnityEngine.Assertions;


public class BossDeathExplosion : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1000.0f)] private float range = 7.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float damage = 5.0f;
    [SerializeField] private GameObject explosionEffectPrefab;
    private HealthComponent health;


    private void Awake()
    {
        Assert.IsNotNull(explosionEffectPrefab);
        health = GetComponent<HealthComponent>();
        Assert.IsNotNull(health);
        health.OnDeath += Explode;
    }

    private void Explode()
    {
        var effectHandler = Instantiate(explosionEffectPrefab, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, range);

        foreach(var collider in colliders)
        {
            HealthComponent health = collider.GetComponent<HealthComponent>();
            if(health != null)
            {
                health.ApplyDamage(damage);
            }
        }
        Destroy(effectHandler, 2.0f);
    }
}