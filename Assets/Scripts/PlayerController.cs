using UnityEngine;
using UnityEngine.Assertions;


public class PlayerController : MonoBehaviour
{
    [SerializeField, Range(0.0f, 1000.0f)] private float speed = 10.0f;
    [SerializeField, Range(0.0f, 1000.0f)] private float sensitivity = 90.0f;
    [SerializeField] private AudioClip footstepClip;
    [SerializeField] private AudioClip playerHitClip;
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
    private CharacterController controller;
    private HealthComponent health;
    private Weapon weapon;
    private AudioSource audioSource;


    public HealthComponent Health => health;

    public float Sensitivity
    {
        get => sensitivity;
        set => sensitivity = value;
    }

    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();
        Assert.IsNotNull(camera);

        controller = GetComponent<CharacterController>();
        Assert.IsNotNull(controller);
        Cursor.lockState = CursorLockMode.Locked;

        health = GetComponent<HealthComponent>();
        Assert.IsNotNull(health);
        health.OnHit += PlayAudioHit;

        weapon = GetComponentInChildren<Weapon>();
        Assert.IsNotNull(weapon);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);

        Assert.IsNotNull(footstepClip);
        Assert.IsNotNull(playerHitClip);

        Assert.IsNotNull(groundCheck);
    }

    private void Update()
    {
        if (health.IsAlive && camera.gameObject.activeSelf)
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

            // Player movement
            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
            if (isGrounded && velocity.y < 0f)
            {
                velocity.y = 0f;
            }
            Vector3 movementVector = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
            if (Input.GetButtonDown("Dash") && Time.time > nextDash)
            {
                nextDash = Time.time + dashCooldown;
                StartCoroutine(Dash(movementVector));
            }

            controller.Move(movementVector * speed * Time.deltaTime);
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            if (!isGrounded)
            {
                velocity.y += gravity * Time.deltaTime;
            }
            controller.Move(velocity * Time.deltaTime);

            // Sound of movement
            if (isGrounded && movementVector.magnitude > 1.0f && !audioSource.isPlaying)
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

    private void PlayAudioHit()
    {
        audioSource.PlayOneShot(playerHitClip);
    }

    public void SwitchCamera()
    {
        camera?.gameObject.SetActive(!camera.gameObject.activeSelf);
    }
}
