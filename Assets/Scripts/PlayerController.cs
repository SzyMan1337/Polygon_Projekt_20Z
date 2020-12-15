using UnityEngine;
using UnityEngine.Assertions;


public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1000.0f)] private float speed = 10.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float sensitivity = 90.0f;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip playerHitClip;
    private Camera camera = null;
    private float yRotation = 0.0f;
    private CharacterController controller;
    private Rigidbody body;
    private HealthComponent health;
    private AudioSource audioSource;
    private Weapon weapon;
    private WeaponManager weaponManager;

    public HealthComponent Health => health;
    public (float, float, float) Position => (body.position.x, body.position.y, body.position.z);

    public WeaponManager WeaponManager => weaponManager;
    public Rigidbody Body => body;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        Assert.IsNotNull(body);

        camera = GetComponentInChildren<Camera>();
        Assert.IsNotNull(camera);

        controller = GetComponent<CharacterController>();
        Assert.IsNotNull(controller);
        Cursor.lockState = CursorLockMode.Locked;

        health = GetComponent<HealthComponent>();
        Assert.IsNotNull(health);
        health.OnHit += PlayAudioHit;

        weaponManager = GetComponentInChildren<WeaponManager>();
        Assert.IsNotNull(weaponManager);

        weapon = weaponManager.FirstWeapon;
        //weapon = GetComponentInChildren<Weapon>();
        Assert.IsNotNull(weapon);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);

        Assert.IsNotNull(footstepClip);
        Assert.IsNotNull(playerHitClip);
    }

    private void PlayAudioHit()
    {
        audioSource.PlayOneShot(playerHitClip);
    }

    private void Update()
    {
        if (health.IsAlive)
        {
            float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
            float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

            yRotation -= mouseY;
            yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);

            // Rotating camera
            camera.transform.localRotation = Quaternion.Euler(yRotation, 0.0f, 0.0f);
            transform.Rotate(Vector3.up * mouseX);

            // Controling gun aimpoint
            var ray = camera.ScreenPointToRay(new Vector2(camera.pixelWidth / 2.0f, camera.pixelHeight / 2.0f));
            if (Physics.Raycast(ray, out var hit))
            {
                weapon.transform.LookAt(hit.point);
            }
            else
            {
                weapon.transform.localRotation = Quaternion.identity;
            }

            if (controller.isGrounded)
            {
                body.velocity = Vector3.zero;
            }

            // Player movement
            Vector3 movementVector = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical") + transform.up * body.velocity.y;
            controller.Move(movementVector * speed * Time.deltaTime);

            // Sound of movement
            if (controller.isGrounded && controller.velocity.magnitude > 2.0f && !audioSource.isPlaying)
            {
                audioSource.volume = Random.Range(0.7f, 1.0f);
                audioSource.pitch = Random.Range(0.7f, 1.0f);
                audioSource.Play();
            }

            // Shooting
            if (Input.GetMouseButton(0))
            {
                weapon.Shoot();
            }

            // Change weapon
            if (Input.GetAxis("Mouse ScrollWheel") != 0 && weaponManager.SecondWeapon != null)
            {
                weapon = weaponManager.ChangeWeapon();
                Debug.Log(weapon.name);
            }

            // Throw weapon
            if (Input.GetKeyDown(KeyCode.E))
            {
                ThrowWeapon(weaponManager.SecondWeapon);
            }

        }
    }
    public void PickUpWeapon(Weapon w)
    {
        weaponManager.SecondWeapon = w;
        weapon = w;
    }

    public void ThrowWeapon(Weapon w)
    {
        if (w == null)
            return;

        if (weapon == weaponManager.SecondWeapon)
            weapon = weaponManager.ChangeWeapon();
        (float xWeapon, _, float zWeapon) = PlayerPointingDirection();
        w.transform.position -= new Vector3(xWeapon*3, 0, zWeapon*3);
        w.DeactivateWeapon();
        weaponManager.SecondWeapon = null;
    }

    public (float, float, float) PlayerPointingDirection()
    {
        return (body.transform.forward.x, body.transform.forward.y, body.transform.forward.z);
    }

    public void PushPlayerBack()
    {
        (float x, _, float z) = PlayerPointingDirection();
        body.transform.position -= new Vector3(x, 0, z);
    }


}
