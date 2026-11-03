using UnityEngine;

public sealed class PlayerLook : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private Transform cameraPivot;

    [Header("Look Settings")]
    [SerializeField, Range(1f, 89f)] private float pitchLimit = 85f;

    private float yaw;
    private float pitch;

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (Cursor.lockState != CursorLockMode.Locked)
        {
            return;
        }

        Vector2 lookInput = input.LookInput;

        yaw += lookInput.x * GameSettings.MouseSensitivity;
        pitch -= lookInput.y * GameSettings.MouseSensitivity;

        yaw = Mathf.Repeat(yaw, 360f);
        pitch = Mathf.Clamp(pitch, -pitchLimit, pitchLimit);

        cameraPivot.localRotation = Quaternion.Euler(pitch, yaw, 0f);
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}