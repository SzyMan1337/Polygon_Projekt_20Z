using UnityEngine;
using UnityEngine.Assertions;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float sensitivity = 90.0f;
    private Transform cameraTransform = null;
    private float yRotation = 0.0f;
    private CharacterController controller;

    private void Awake()
    {
        var camera = GetComponentInChildren<Camera>();
        Assert.IsNotNull(camera);
        cameraTransform = camera.transform;

        controller = GetComponent<CharacterController>();
        Assert.IsNotNull(controller);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        yRotation -= mouseY;
        yRotation = Mathf.Clamp(yRotation, -90.0f, 90.0f);

        cameraTransform.localRotation = Quaternion.Euler(yRotation, 0.0f, 0.0f);
        transform.Rotate(Vector3.up * mouseX);

        Vector3 movementVector = transform.right * Input.GetAxis("Horizontal") + transform.forward * Input.GetAxis("Vertical");
        controller.Move(movementVector * speed * Time.deltaTime);
    }
}
