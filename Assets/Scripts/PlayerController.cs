using UnityEngine;
using UnityEngine.Assertions;


public class PlayerController : MonoBehaviour
{
    [SerializeField] private Settings settings;
    [SerializeField, Range(0.0f, 1000.0f)] private float speed = 10.0f;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundMask;
    [SerializeField] private float gravity = -19.62f;
    [SerializeField] private float groundDistance = 0.2f;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float dashSpeed = 20f;
    [SerializeField] private float dashTime = 0.25f;
    [SerializeField] private float dashCooldown = 1.25f;
    private float nextDash = -1f;
    private bool isGrounded;
    private Vector3 velocity;
    private Camera camera = null;
    private float yRotation = 0.0f;
    private AudioSource audioSource = null;
    private CharacterController controller;
    private HealthComponent health;
    private WeaponManager weaponManager;
    private Vector3 movementVector;

    public HealthComponent Health => health;
    public WeaponManager WeaponManager => weaponManager;
    public Camera Camera => camera;
    public float Speed { set; get; }


    private void Awake()
    {
        Assert.IsNotNull(settings);
        Assert.IsNotNull(groundCheck);

        camera = GetComponentInChildren<Camera>();
        Assert.IsNotNull(camera);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);

        controller = GetComponent<CharacterController>();
        Assert.IsNotNull(controller);
        Cursor.lockState = CursorLockMode.Locked;

        health = GetComponent<HealthComponent>();
        Assert.IsNotNull(health);

        weaponManager = GetComponentInChildren<WeaponManager>();
        Assert.IsNotNull(weaponManager);
    }

    private void Update()
    {
        if (health.IsAlive && camera.gameObject.activeSelf)
        {
            LookingUpdate();
            MovementUpdate();
            if (weaponManager.HasWeapon)
            {
                WeaponsUpdate();
            }
        }
    }

    private void LookingUpdate()
    {
        float mouseX = Input.GetAxis("Mouse X") * settings.MouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * settings.MouseSensitivity * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);

        camera.transform.localRotation = Quaternion.Euler(yRotation, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    private void MovementUpdate()
    {
        //Falling down
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGrounded)
        {
            if (velocity.y < 0f)
            {
                velocity.y = 0f;
            }
        }
        else
        {
            velocity.y += gravity * Time.deltaTime;
        }
        
        movementVector = Vector3.zero;
        //Movement
        if(Input.GetButton("Horizontal") || Input.GetButton("Vertical"))
        {
            movementVector = (transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical")).normalized;
            controller.Move(movementVector * speed * Time.deltaTime);
        }

        //Dash
        if (Input.GetButtonDown("Dash") && Time.time > nextDash)
        {
            nextDash = Time.time + dashCooldown;
            StartCoroutine(Dash(movementVector));
        }

        //Jump
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }
        
        controller.Move(velocity * Time.deltaTime);

        // Sound of movement
        if (isGrounded && movementVector.magnitude > 1.0f && !audioSource.isPlaying)
        {
            audioSource.volume = Random.Range(0.7f, 1.0f);
            audioSource.pitch = Random.Range(0.7f, 1.0f);
            audioSource.Play();
        }
    }

    private void WeaponsUpdate()
    {
        var ray = camera.ScreenPointToRay(new Vector2(camera.pixelWidth / 2.0f, camera.pixelHeight / 2.0f));
        if (Physics.Raycast(ray, out var hit))
        {
            weaponManager.transform.LookAt(hit.point);
        }
        else
        {
            weaponManager.transform.localRotation = Quaternion.identity;
        }

        // Shooting
        if (Input.GetMouseButton(0))
        {
            weaponManager.ShootCurrentWeapon();
        }

        // Weapon change
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            weaponManager.CycleWeaponUp();
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            weaponManager.CycleWeaponDown();
        }

        // Weapon drop
        if (Input.GetKeyDown(KeyCode.Q))
        {
            weaponManager.DropCurrentWeapon();
        }
    }

    private System.Collections.IEnumerator Dash(Vector3 movementVector)
    {
        float startTime = Time.time;
        while (Time.time < startTime + dashTime)
        {
            controller.Move(movementVector * dashSpeed * Time.deltaTime);
            yield return null;
        }
    }

    public void ToggleCamera()
    {
        camera?.gameObject.SetActive(!camera.gameObject.activeSelf);
    }
}
