using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private Transform playerCamera;

    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float mouseSensitivity = 0.1f;

    private Rigidbody rb;
    private Vector2 moveInput;
    private float yaw;
    private float pitch;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        yaw = transform.eulerAngles.y;
    }

    private void OnEnable()
    {
        moveAction.action.Enable();
        lookAction.action.Enable();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        moveInput = moveAction.action.ReadValue<Vector2>();

        Vector2 mouseDelta = lookAction.action.ReadValue<Vector2>();

        yaw += mouseDelta.x * mouseSensitivity;
        pitch -= mouseDelta.y * mouseSensitivity;
        pitch = Mathf.Clamp(pitch, -85f, 85f);

        playerCamera.rotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void FixedUpdate()
    {
        
        Quaternion facing = Quaternion.Euler(0f, yaw, 0f);

        Vector2 input = Vector2.ClampMagnitude(moveInput, 1f);
        Vector3 movement = facing * new Vector3(input.x, 0f, input.y);

        Vector3 velocity = rb.linearVelocity;
        velocity.x = movement.x * moveSpeed;
        velocity.z = movement.z * moveSpeed;

        
        rb.linearVelocity = velocity;
    }

    private void OnDisable()
    {
        moveAction.action.Disable();
        lookAction.action.Disable();

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}