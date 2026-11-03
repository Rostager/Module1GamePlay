using UnityEngine;
using UnityEngine.InputSystem;

public sealed class PauseMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private InputActionReference pauseAction;
    [SerializeField] private GameObject pausePanel;
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerInputReader playerInput;

    private bool isPaused;

    private void OnEnable()
    {
        pauseAction.action.Enable();
    }

    private void OnDisable()
    {
        pauseAction.action.Disable();

        if (isPaused)
        {
            Time.timeScale = 1f;
        }
    }

    private void Update()
    {
        if (playerHealth.IsDead || !pauseAction.action.WasPressedThisFrame())
        {
            return;
        }

        if (!isPaused)
        {
            Pause();
        }
        else if (optionsMenu.gameObject.activeSelf)
        {
            optionsMenu.Close();
        }
        else
        {
            Resume();
        }
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0f;

        pausePanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Resume()
    {
        GameSettings.Save();
        playerInput.ConsumeJumpPress();

        optionsMenu.gameObject.SetActive(false);
        pausePanel.SetActive(false);

        isPaused = false;
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}