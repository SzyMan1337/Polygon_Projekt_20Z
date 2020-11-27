using UnityEngine;
using UnityEngine.Assertions;
using System.IO;

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
    private Weapon weapon;
    private AudioSource audioSource;


    public HealthComponent Health => health;


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

        weapon = GetComponentInChildren<Weapon>();
        Assert.IsNotNull(weapon);

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
    }

    public void PlayAudioHit()
    {
        audioSource.PlayOneShot(playerHitClip);
    }

    public void Update()
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
            if (controller.isGrounded && controller.velocity.magnitude > 2f && !audioSource.isPlaying)
            {
                audioSource.volume = Random.Range(0.7f, 1f);
                audioSource.pitch = Random.Range(0.7f, 1f);
                audioSource.Play();
            }

            // Shooting
            if (Input.GetMouseButton(0))
            {
                weapon.Shoot();
            }
        }
    }
}
