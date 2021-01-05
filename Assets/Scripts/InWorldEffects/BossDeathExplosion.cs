using UnityEngine;


public class BossDeathExplosion : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1000.0f)] private float range = 7.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float damage = 5.0f;


    private void Awake()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach(var collider in colliders)
        {
            HealthComponent health = collider.GetComponent<HealthComponent>();
            if(health != null)
            {
                health.ApplyDamage(damage);
            }
        }
    }
}