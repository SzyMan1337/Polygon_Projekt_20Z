using UnityEngine;
using UnityEngine.Assertions;


public class Weapon : MonoBehaviour
{
    //[SerializeField] private Projectile projectilePrefab = null;
    //[SerializeField] private Transform barrelEnd = null;
    //[SerializeField, Range(0.0f, 10.0f)] private float timeBetweenShots = 0.2f;
    //private float shotCooldown = 0.0f;
    //private float damage = 5.0f;
    //private AudioSource audioSource;
    //public bool onGround = true;

    [SerializeField] protected Projectile projectilePrefab = null;
    [SerializeField] protected Transform barrelEnd = null;
    [SerializeField, Range(0.0f, 10.0f)] protected float timeBetweenShots = 0.2f;
    protected float shotCooldown = 0.0f;
    protected float damage = 5.0f;
    protected AudioSource audioSource;
    public bool onGround = true;

    protected virtual void Awake()
    {
        Assert.IsNotNull(projectilePrefab);
        Assert.IsNotNull(barrelEnd);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
    }

    protected virtual void Update()
    {
        if(shotCooldown > 0.0f)
            shotCooldown -= Time.deltaTime;

        // Picking up weapon
        if (onGround && MatchingCords())
        {
            PlayerController player = SceneManager.Instance.Player;
            AttatchWeaponToPlayer();
            player.ThrowWeapon(player.WeaponManager.SecondWeapon);
            player.PickUpWeapon(this);
            Debug.Log("got new weapon");
        }
    }

    protected void AttatchWeaponToPlayer()
    {
        PlayerController player = SceneManager.Instance.Player;
        onGround = false;
        (float x, float y, float z) = (-0.7200611f, 0.25f, 0.5f);
        transform.SetParent(player.WeaponManager.transform);
        transform.localPosition = new Vector3(x, y, z);
        transform.localRotation = Quaternion.identity;
    }

    public void Shoot()
    {
        if (shotCooldown <= 0.0f)
        {
            shotCooldown = timeBetweenShots;
            var projectile = Instantiate(projectilePrefab, barrelEnd.position, barrelEnd.rotation);
            projectile.Damage = damage;
            audioSource.Play();
        }
    }

    protected bool MatchingCords()
    {
        float margin = 1.0f;
        (float xP, float yP, float zP) = SceneManager.Instance.Player.Position;
        (float xW, float yW, float zW) = (barrelEnd.position.x, barrelEnd.position.y, barrelEnd.position.z);

        if (Mathf.Abs(xP - xW) < margin && Mathf.Abs(yP - yW) < margin && Mathf.Abs(zP - zW) < margin)
            return true;

        return false;
    }

    public void DeactivateWeapon()
    {
        transform.SetParent(null);
        onGround = true;
        gameObject.SetActive(true);
    }

}