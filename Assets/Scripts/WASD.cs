using UnityEngine;

public class WASD : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;

    [Header("Camera Settings")]
    public Transform playerCamera;
    public float mouseSensitivity = 2f;
    public float maxLookAngle = 80f;

    private Rigidbody rb;
    private float rotationX = 0f;
    private float rotationY = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Prevent physics forces from tipping over the player capsule
        if (rb != null)
        {
            rb.freezeRotation = true;
        }

        // Lock cursor to the screen center
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // --- Independent Camera Mouse Look ---
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

        rotationY += mouseX;
        rotationX -= mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxLookAngle, maxLookAngle);

        if (playerCamera != null)
        {
            playerCamera.rotation = Quaternion.Euler(rotationX, rotationY, 0f);
        }
    }

    void FixedUpdate()
    {
        // --- Rigidbody WASD Movement ---
        float moveFB = Input.GetAxis("Vertical");
        float moveLR = Input.GetAxis("Horizontal");

        Vector3 forward = playerCamera != null ? playerCamera.forward : transform.forward;
        Vector3 right = playerCamera != null ? playerCamera.right : transform.right;

        forward.y = 0f;
        right.y = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDir = (forward * moveFB + right * moveLR).normalized;

        Vector3 targetVelocity = moveDir * moveSpeed;
        targetVelocity.y = rb.velocity.y;
        rb.velocity = targetVelocity;
    }
}