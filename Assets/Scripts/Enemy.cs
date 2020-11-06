using UnityEngine;
using UnityEngine.Assertions;


public class Enemy : MonoBehaviour
{
    [SerializeField] private float speed = 2.0f;
    private Rigidbody rigidbody;

    private const float MOVEMENT_THRESHOLD = 2.0f;

    public Vector3 Destination { get; set; }


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(rigidbody);
    }

    private void Update()
    {
        if(Vector3.Distance(transform.position, Destination) > MOVEMENT_THRESHOLD)
        {
            rigidbody.velocity = (Destination - transform.position).normalized * speed;
        }
    }
}