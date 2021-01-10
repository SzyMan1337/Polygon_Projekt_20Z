using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BazookaProjectile : Projectile
{
    [SerializeField] private float range = 5.0f;
    [SerializeField] private GameObject effectToSpawnPrefab = null;


    private void OnTriggerEnter(Collider collision)
    {
        // Not affecting other triggers
        if (collision.isTrigger)
            return;

        Instantiate(effectToSpawnPrefab, transform.position, transform.rotation);

        // Apply damage to hit object
        HealthComponent health = collision.gameObject.GetComponentInParent<HealthComponent>();
        if (health != null)
        {
            health.ApplyDamage(Damage);
        }

        // Apply damage to near objects 
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (var collider in colliders)
        {
            health = collider.GetComponent<HealthComponent>();
            if (health != null)
            {
                health.ApplyDamage(Damage);
            }
        }
        Destroy(this.gameObject);
    }
}
