using UnityEngine;
using UnityEngine.Assertions;


public class Weapon : MonoBehaviour
{
    [SerializeField] private Projectile projectilePrefab = null;
    [SerializeField] private Transform barrelEnd = null;
    [SerializeField, Range(0.0f, 10.0f)] private float timeBetweenShots = 0.2f;
    private float shotCooldown = 0.0f;
    protected float damage = 5.0f;
    private AudioSource audioSource;
    private bool onGround = true;
    private readonly float pickUpRange = 2.0f;


    public bool OnGround
    {
        get
        {
            return onGround;
        }
        set
        {
            onGround = value;
        }
    }

    protected void Awake()
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

        // Picking up weapon
        Vector3 distanceToPlayer = SceneManager.Instance.Player.transform.position - transform.position;
        if (onGround && distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E)) 
        {
            SceneManager.Instance.Player.PickUpWeapon(this);
            onGround = false;
        }

    }

    public virtual void Shoot() 
    {
        if (shotCooldown <= 0.0f)
        {
            shotCooldown = timeBetweenShots;
            var projectile = Instantiate(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
            projectile.Damage = damage;
            audioSource.Play();
        }
    }

    public virtual void DetachWeapon()
    {
        // Set parent to null
        transform.SetParent(null);
        onGround = true;
    }

}