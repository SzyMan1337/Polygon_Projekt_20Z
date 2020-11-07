using UnityEngine;
using UnityEngine.Assertions;


public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float lifetime = 5.0f;
    private Rigidbody body = null;


    private void Awake()
    {
        body = GetComponent<Rigidbody>();
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

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}