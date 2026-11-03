using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerMotor : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform cameraPivot;
    [SerializeField] private Transform groundCheck;

    [Header("Ground Detection")]
    [SerializeField] private LayerMask groundLayers;
    [SerializeField] private float groundCheckRadius = 0.12f;

    [Header("Ground Movement")]
    [SerializeField] private float groundSpeed = 10f;
    [SerializeField] private float groundAcceleration = 80f;
    [SerializeField] private float groundFriction = 8f;
    [SerializeField] private float stopSpeed = 4f;

    [Header("Jump Settings")]
    [SerializeField] private float jumpHeight = 2f;

    private Rigidbody body;

    public bool IsGrounded { get; private set; }

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    public void CheckGround()
    {
        IsGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayers, QueryTriggerInteraction.Ignore);
    }

    public void MoveOnGround(Vector2 moveInput)
    {
        ApplyGroundFriction();

        Vector3 moveDirection = GetMoveDirection(moveInput);
        float targetSpeed = groundSpeed * Mathf.Clamp01(moveInput.magnitude);

        Accelerate(moveDirection, targetSpeed, groundAcceleration);
    }

    private Vector3 GetMoveDirection(Vector2 moveInput)
    {
        Quaternion facing = Quaternion.Euler(0f, cameraPivot.eulerAngles.y, 0f);
        Vector3 direction = facing * new Vector3(moveInput.x, 0f, moveInput.y);

        return direction.normalized;
    }

    private void ApplyGroundFriction()
    {
        Vector3 velocity = body.linearVelocity;
        Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

        float speed = horizontalVelocity.magnitude;

        if (speed <= 0f)
        {
            return;
        }

        float frictionSpeed = Mathf.Max(speed, stopSpeed);
        float speedDrop = frictionSpeed * groundFriction * Time.fixedDeltaTime;
        float newSpeed = Mathf.Max(speed - speedDrop, 0f);

        horizontalVelocity *= newSpeed / speed;

        body.linearVelocity = new Vector3(horizontalVelocity.x, velocity.y, horizontalVelocity.z);
    }

    private void Accelerate(Vector3 direction, float targetSpeed, float acceleration)
    {
        if (direction.sqrMagnitude == 0f)
        {
            return;
        }

        Vector3 velocity = body.linearVelocity;

        float currentSpeed = Vector3.Dot(velocity, direction);
        float speedNeeded = targetSpeed - currentSpeed;

        if (speedNeeded <= 0f)
        {
            return;
        }

        float speedToAdd = acceleration * Time.fixedDeltaTime;
        speedToAdd = Mathf.Min(speedToAdd, speedNeeded);

        body.linearVelocity = velocity + direction * speedToAdd;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
        {
            return;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}