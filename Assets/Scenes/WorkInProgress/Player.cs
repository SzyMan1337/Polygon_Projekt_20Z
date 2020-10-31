using UnityEngine;


public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f;
    [SerializeField] private float sensitivity = 90.0f;
    private Transform playerBody = null;
    private float xRotation = 0f;
    private CharacterController controller;


    // Start is called before the first frame update
    private void Start()
    {
       Cursor.lockState = CursorLockMode.Locked;
    }

   

    public void Update()
    {
        
        float mouseX = Input.GetAxis("Mouse X") *sensitivity *Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") *sensitivity *Time.deltaTime; 

        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        var movementVector = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")).normalized;
        

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f,0f);
        playerBody.Rotate(Vector3.up * mouseX);

        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move( move * speed * Time.deltaTime);
    }
}


