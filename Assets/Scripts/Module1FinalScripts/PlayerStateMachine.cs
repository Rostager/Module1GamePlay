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

    [Header("Debug")]
    [SerializeField] private MovementState currentState = MovementState.Airborne;

    public MovementState CurrentState => currentState;

    private void FixedUpdate()
    {
        motor.CheckGround();
        UpdateState();

        switch (currentState)
        {
            case MovementState.Grounded:
                motor.MoveOnGround(input.MoveInput);
                break;

            case MovementState.Airborne:
                // Gravity handles falling until we add air movement.
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
}