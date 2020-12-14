using UnityEngine;
using UnityEngine.Assertions;


public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab = null;
    [SerializeField] private Transform barrelEnd = null;
    [SerializeField, Range(0.0f, 10.0f)] private float timeBetweenShots = 0.2f;
    private float shotCooldown = 0.0f;
    private AudioSource audioSource;


    private void Awake()
    {
        Assert.IsNotNull(projectilePrefab);
        Assert.IsNotNull(barrelEnd);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
    }

    private void Update()
    {
        if(shotCooldown > 0.0f)
            shotCooldown -= Time.deltaTime;
    }

    public void Shoot()
    {
        if (shotCooldown <= 0.0f)
        {
            shotCooldown = timeBetweenShots;
            var projectile = Instantiate(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
            audioSource.Play();
        }
    }
}