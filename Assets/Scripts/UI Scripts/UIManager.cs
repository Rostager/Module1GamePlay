using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject gameplay;
    [SerializeField] private GameObject options;
    [SerializeField] private GameObject credits;
    [SerializeField] private GameObject gameOver;
    [SerializeField] private TMP_Text timeSurvivedText;

    private void Start()
    {
        mainMenu.SetActive(true);
        gameplay.SetActive(false);
        options.SetActive(false);
        credits.SetActive(false);
        gameOver.SetActive(false);
    }

    public void StartGame()
    {
        mainMenu.SetActive(false);
        gameplay.SetActive(true);
        options.SetActive(false);
        credits.SetActive(false);
        gameOver.SetActive(false);
    }

    public void OpenOptions()
    {
        mainMenu.SetActive(false);
        gameplay.SetActive(false);
        options.SetActive(true);
        credits.SetActive(false);
        gameOver.SetActive(false);

    }

    public void OpenCredits()
    {
        mainMenu.SetActive(false);
        gameplay.SetActive(false);
        options.SetActive(false);
        credits.SetActive(true);
        gameOver.SetActive(false);
    }

    public void OpenMainMenu()
    {
        mainMenu.SetActive(true);
        gameplay.SetActive(false);
        options.SetActive(false);
        credits.SetActive(false);
        gameOver.SetActive(false);
    }

    public void OpenGameOver(float timeSurvived)
    {
        timeSurvivedText.text = $"{timeSurvived:F1}s";

        mainMenu.SetActive(false);
        gameplay.SetActive(false);
        options.SetActive(false);
        credits.SetActive(false);
        gameOver.SetActive(true);
    }
}
