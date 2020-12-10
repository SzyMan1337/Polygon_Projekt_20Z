using UnityEngine;
using UnityEngine.Assertions;


public class Enemy : MonoBehaviour
{
    [SerializeField] private Weapon weapon = null;
    [SerializeField, Range(0.0f, 1000.0f)] private float speed = 2.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float aimRange = 50.0f;
    [SerializeField, Range(0.0f, 10.0f)] private float aimSpeed = 0.1f;
    [SerializeField] private AudioClip enemyHitClip;
    [SerializeField] private GameObject enemyDeathPrefab;
    private Rigidbody rigidbody = null;
    private const float MOVEMENT_THRESHOLD = 20.0f;
    private HealthComponent health;
    private AudioSource audioSource;


    public static event System.Action OnAnyEnemyDeath;


    public HealthComponent Health => health;
    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        Assert.IsNotNull(rigidbody);

        weapon = GetComponentInChildren<Weapon>();
        Assert.IsNotNull(weapon);

        health = GetComponent<HealthComponent>();
        Assert.IsNotNull(health);
        health.OnDeath += OnAnyEnemyDeath;
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
            if (Vector3.Distance(transform.position, player.transform.position) > MOVEMENT_THRESHOLD)
            {
                rigidbody.velocity = (player.transform.position - transform.position).normalized * speed;
            }
            else
            {
                rigidbody.velocity = Vector3.zero;
            }

            // Rotating towards player
            var rotationVector = new Vector3(transform.rotation.x, Quaternion.LookRotation(player.transform.position - transform.position, transform.up).eulerAngles.y, transform.rotation.z);
            transform.rotation = Quaternion.Euler(rotationVector);

            //Shooting at player
            if (Physics.Raycast(transform.position, transform.forward, out var hit, aimRange))
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

    private void OnDestroy()
    {
        health.OnDeath -= OnAnyEnemyDeath;
    }

    private void PlayAudioOnHit()
    {
        audioSource.PlayOneShot(enemyHitClip);
    }

    private void PlayAudioOnDeath()
    {
        var enemyDeath = Instantiate(enemyDeathPrefab, transform.position, transform.rotation);
        Destroy(enemyDeath, 0.8f);
    }
}