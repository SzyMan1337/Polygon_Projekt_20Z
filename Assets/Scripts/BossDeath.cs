using UnityEngine;

public class BossDeath : MonoBehaviour
{ 
    public GameObject explosionEffect;
    public GameObject effectHandler;
    [SerializeField] private float radius = 7.0f;
    [SerializeField] private float power = 500.0f;

    private void OnDestroy()
    {
        // Efekt wybuchu
        effectHandler = Instantiate(explosionEffect, transform.position, transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        foreach(var collider in colliders)
        {
            HealthComponent health = collider.GetComponent<HealthComponent>();
            if(health != null)
            {
                // ew dodanie siy ale nie wiem czy to ma sens
                health.ApplyDamage((radius - Vector3.Distance(collider.transform.position, transform.position)) * power);
            }
        }
        Destroy(effectHandler, 2.0f);
    }
}