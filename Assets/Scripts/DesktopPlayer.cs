using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class DesktopController : MonoBehaviour
{
    [Header("Movement Speeds")]
    public float walkSpeed = 4f;
    public float sprintSpeed = 8f;
    public float slowSpeed = 2f;

    [Header("Mouse Look")]
    public float mouseSensitivity = 2.5f;
    public Transform cameraTransform;

    [Header("Gravity & Jump")]
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;

    CharacterController controller;
    float yVelocity;
    float xRotation = 0f;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        HandleMouseLook();
        HandleMovement();
    }

    void HandleMouseLook()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * 100f * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * 100f * Time.deltaTime;

        // Vertical (camera)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -85f, 85f);
        cameraTransform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Horizontal (player body)
        transform.Rotate(Vector3.up * mouseX);
    }

    void HandleMovement()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        // Speed control
        float speed = walkSpeed;

        if (Input.GetKey(KeyCode.LeftShift))
            speed = sprintSpeed;
        else if (Input.GetKey(KeyCode.LeftControl))
            speed = slowSpeed;

        // Apply horizontal movement
        Vector3 horizontalMove = move * speed;

        // Ground check
        if (controller.isGrounded && yVelocity < 0)
        {
            yVelocity = -2f; // keeps player grounded
        }

        // Jump
        if (Input.GetKeyDown(KeyCode.Space) && controller.isGrounded)
        {
            yVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // Gravity
        yVelocity += gravity * Time.deltaTime;

        Vector3 verticalMove = Vector3.up * yVelocity;

        // Final movement
        controller.Move((horizontalMove + verticalMove) * Time.deltaTime);
    }
}