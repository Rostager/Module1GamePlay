using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PlayerInputReader : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction;
    [SerializeField] private InputActionReference lookAction;
    [SerializeField] private InputActionReference jumpAction;

    public Vector2 MoveInput =>
        moveAction.action.ReadValue<Vector2>();

    public Vector2 LookInput =>
        lookAction.action.ReadValue<Vector2>();

    public bool JumpHeld =>
        jumpAction.action.IsPressed();

    public float LastJumpPressedTime { get; private set; }
        = float.NegativeInfinity;

    private void OnEnable()
    {
        jumpAction.action.performed += OnJumpPerformed;

        moveAction.action.Enable();
        lookAction.action.Enable();
        jumpAction.action.Enable();
    }

    private void OnDisable()
    {
        jumpAction.action.performed -= OnJumpPerformed;

        moveAction.action.Disable();
        lookAction.action.Disable();
        jumpAction.action.Disable();

        ConsumeJumpPress();
    }

    private void OnJumpPerformed(InputAction.CallbackContext context)
    {
        LastJumpPressedTime = Time.time;
    }

    public void ConsumeJumpPress()
    {
        LastJumpPressedTime = float.NegativeInfinity;
    }
}