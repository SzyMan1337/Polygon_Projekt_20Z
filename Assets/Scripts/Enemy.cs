using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Weapon weapon = null;
    [SerializeField, Range(0, 100)] private int valueInPoints = 1;
    [SerializeField, Range(0.0f, 1000.0f)] private float speed = 2.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float shootingRange = 50.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float distanceToTarget = 20.0f;
    private new Rigidbody rigidbody = null;
    private HealthComponent health = null;
    private NavMeshAgent navMeshAgent;


    public static event System.Action<Enemy> OnAnyEnemyDeath;


    public HealthComponent Health => health;
    public int ValueInPoints => valueInPoints;


    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(rigidbody);

        weapon = GetComponentInChildren<Weapon>();
        Assert.IsNotNull(weapon);

        health = GetComponent<HealthComponent>();
        Assert.IsNotNull(health);
        health.OnDeath += () => { OnAnyEnemyDeath?.Invoke(this); };

        navMeshAgent = GetComponent<NavMeshAgent>();
        Assert.IsNotNull(navMeshAgent);
        navMeshAgent.speed = speed;
        navMeshAgent.stoppingDistance = Random.Range(0, distanceToTarget);
    }

    private void Update()
    {
        var player = SceneManager.Instance?.Player;
        if (player != null && player.Health.IsAlive)
        {
            navMeshAgent.SetDestination(player.transform.position);
            
            //Shooting at player
            if (Physics.Raycast(weapon.transform.position, player.transform.position - weapon.transform.position, out var hit, shootingRange))
            {
                if (hit.collider.gameObject == player.gameObject)
                {
                    weapon.transform.LookAt(hit.point);
                    weapon.Shoot();
                }
                else
                {
                    weapon.transform.localRotation = Quaternion.identity;
                }
            }
            else
            {
                weapon.transform.localRotation = Quaternion.identity;
            }
        }   
    }
}