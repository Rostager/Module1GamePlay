using UnityEngine;
using UnityEngine.SceneManagement;

public sealed class MainMenu : MonoBehaviour
{
    [SerializeField] private string gameplaySceneName = "Gameplay";

    private bool isLoading;

    private void Start()
    {
        Time.timeScale = 1f;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void PlayGame()
    {
        if (isLoading)
        {
            return;
        }

        isLoading = true;
        Time.timeScale = 1f;

        SceneManager.LoadSceneAsync(gameplaySceneName);
    }

}