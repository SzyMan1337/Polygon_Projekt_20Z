using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.AI;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Weapon weapon = null;
    [SerializeField, Range(0, 100)] private int valueInPoints = 1;
    [SerializeField, Range(0.0f, 1000.0f)] private float shootingRange = 50.0f;
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
        navMeshAgent.stoppingDistance = shootingRange - 0.2f;
    }

    private void Update()
    {
        var player = SceneManager.Instance?.Player;
        if (player != null && player.Health.IsAlive)
        {
            if (Physics.Raycast(weapon.transform.position, player.transform.position - weapon.transform.position, out var hit, shootingRange))
            {
                if (hit.collider.gameObject == player.gameObject)
                {
                    weapon.transform.LookAt(hit.point);
                    weapon.Shoot();
                    navMeshAgent.isStopped = true;
                    var rotationVector = new Vector3(transform.rotation.x, 
                           Quaternion.LookRotation(player.transform.position - transform.position, transform.up).eulerAngles.y, transform.rotation.z);
                    transform.rotation = Quaternion.Euler(rotationVector);
                }
                else
                {
                    weapon.transform.localRotation = Quaternion.identity;
                    navMeshAgent.isStopped = false;
                    navMeshAgent.SetDestination(player.transform.position);
                }
            }
            else
            {
                weapon.transform.localRotation = Quaternion.identity;
                navMeshAgent.isStopped = false;
                navMeshAgent.SetDestination(player.transform.position);
            }
        }   
    }
}