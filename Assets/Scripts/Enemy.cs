using UnityEngine;
using UnityEngine.Assertions;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Weapon weapon = null;
    [SerializeField, Range(0, 100)] private int valueInPoints = 1;
    [SerializeField, Range(0.0f, 1000.0f)] private float speed = 2.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float shootingRange = 50.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float distanceToTarget = 20.0f;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private GameObject enemyDeathPrefab;
    private Rigidbody rigidbody = null;
    private HealthComponent health;
    private AudioSource audioSource;


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
        health.OnDeath += PlayAudioOnDeath;
        health.OnHit += PlayAudioOnHit;

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);

        Assert.IsNotNull(enemyHitClip);
        Assert.IsNotNull(enemyDeathPrefab);
    }

    private void Update()
    {
        var player = SceneManager.Instance?.Player;
        if (player != null && player.Health.IsAlive)
        {
            // Movement towards player
            if (Vector3.Distance(transform.position, player.transform.position) > distanceToTarget)
            {
                var velocityVector = (player.transform.position - transform.position).normalized * speed;
                velocityVector.y = 0.0f;
                rigidbody.velocity = velocityVector;
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
            }

            // Rotating towards player
            var rotationVector = new Vector3(transform.rotation.x, Quaternion.LookRotation(player.transform.position - transform.position, transform.up).eulerAngles.y, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotationVector);

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

    private void PlayAudioOnHit()
    {
        audioSource.PlayOneShot(enemyHitClip);
    }

    private void PlayAudioOnDeath()
    {
        var enemyDeath = Instantiate(enemyDeathPrefab, transform.position, transform.rotation);
        Destroy(enemyDeath, 1.6f);
    }
}