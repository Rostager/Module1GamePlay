using UnityEngine;

public sealed class PlayerStateMachine : MonoBehaviour
{
    public enum MovementState
    {
        Grounded,
        Airborne
    }

    [Header("References")]
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private PlayerMotor motor;

    [Header("Jump Timing")]
    [SerializeField] private float jumpBufferTime = 0.12f;
    [SerializeField] private float coyoteTime = 0.1f;

    [Header("Debug")]
    [SerializeField] private MovementState currentState = MovementState.Airborne;

    private float lastGroundedTime = float.NegativeInfinity;

    public MovementState CurrentState => currentState;

    private void FixedUpdate()
    {
        motor.CheckGround();
        UpdateState();

        if (currentState == MovementState.Grounded)
        {
            lastGroundedTime = Time.time;
        }

        TryJump();

        switch (currentState)
        {
            case MovementState.Grounded:
                motor.MoveOnGround(input.MoveInput);
                break;

            case MovementState.Airborne:
                motor.MoveInAir(input.MoveInput);
                break;
        }
    }

    private void UpdateState()
    {
        if (motor.IsGrounded)
        {
            currentState = MovementState.Grounded;
        }
        else
        {
            currentState = MovementState.Airborne;
        }
    }

    private void TryJump()
    {
        bool hasBufferedPress = Time.time - input.LastJumpPressedTime <= jumpBufferTime;
        bool withinCoyoteTime = Time.time - lastGroundedTime <= coyoteTime;
        bool grounded = currentState == MovementState.Grounded;

        bool canJump = grounded || withinCoyoteTime;
        bool wantsJump = hasBufferedPress || (grounded && input.JumpHeld);

        if (!canJump || !wantsJump)
        {
            return;
        }

        motor.Jump();
        input.ConsumeJumpPress();

        lastGroundedTime = float.NegativeInfinity;
        currentState = MovementState.Airborne;
    }
}