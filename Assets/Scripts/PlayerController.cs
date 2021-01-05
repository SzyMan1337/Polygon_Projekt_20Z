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
    public Camera Camera => camera;

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

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);

        Assert.IsNotNull(footstepClip);
        Assert.IsNotNull(playerHitClip);
    }

    private void Start()
    {
        weapon = weaponManager.CurrentWeapon;
        Assert.IsNotNull(weapon);
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
                weaponManager.transform.LookAt(hit.point);
            }
            else
            {
                //weapon.transform.localRotation = Quaternion.identity;
                weaponManager.transform.localRotation = Quaternion.identity;
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
            if (Input.GetMouseButtonDown(0))
            {
                weapon.Shoot();
            }

            // Weapon change
            if (Input.GetAxis("Mouse ScrollWheel") > 0)
            {
                weaponManager.ChangeCurrentWeaponUp();
                weapon = weaponManager.CurrentWeapon;
            }

            if (Input.GetAxis("Mouse ScrollWheel") < 0)
            {
                weaponManager.ChangeCurrentWeaponDown();
                weapon = weaponManager.CurrentWeapon;
            }

            // Weapon drop
            if (Input.GetKeyDown(KeyCode.Q))
            {
                DropWeapon();
                weapon = weaponManager.CurrentWeapon;
            }

        }
    }

    public void DropWeapon()
    {
        weaponManager.DetachCurrentWeapon();
        weapon = weaponManager.CurrentWeapon;
    }

    public void PickUpWeapon(Weapon w)
    {
        weaponManager.AddWeapon(w);
        weapon = weaponManager.CurrentWeapon;
    }

}
