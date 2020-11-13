using UnityEngine;
using UnityEngine.Assertions;


public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1000.0f)] private float speed = 10.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float lifetime = 5.0f;
    [SerializeField] private float damage = 10.0f;


    public float Damage
    {
        get => damage;
        set => damage = value;
    }


    private void Awake()
    {
        var body = GetComponent<Rigidbody>();
        Assert.IsNotNull(body);
        body.velocity = transform.forward * speed;
    }

    private void Update()
    {
        if(lifetime > 0.0f)
        {
            lifetime -= Time.deltaTime;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        // Not affecting other triggers
        if (collision.isTrigger) 
            return;
        
        // Giving damage
        var objectHealth = collision.gameObject.GetComponent<HealthComponent>();
        if (objectHealth != null)
        {
            objectHealth.ApplyDamage(Damage);
        }  

        Destroy(gameObject);
    }
}