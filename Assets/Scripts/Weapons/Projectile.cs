using UnityEngine;
using UnityEngine.Assertions;


public class Projectile : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1000.0f)] private float speed = 10.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float lifetime = 5.0f;


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

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }
}