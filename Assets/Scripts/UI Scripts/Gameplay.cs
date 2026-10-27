using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class Gameplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image hpFill;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text promptText;
    [SerializeField] private UIManager uiManager;

    [Header("Gameplay Settings")]
    [SerializeField] private float maxHP = 100f;
    [SerializeField] private float hpPerPress = 8f;
    [SerializeField] private float startingDrain = 15f;
    [SerializeField] private float drainIncreasePerSecond = 0.5f;

    [Header("Key Images")]
    [SerializeField] private Image firstKeyImage;
    [SerializeField] private Image secondKeyImage;

    [Header("A Sprites")]
    [SerializeField] private Sprite aReleased;
    [SerializeField] private Sprite aPressed;

    [Header("D Sprites")]
    [SerializeField] private Sprite dReleased;
    [SerializeField] private Sprite dPressed;

    [Header("S Sprites")]
    [SerializeField] private Sprite sReleased;
    [SerializeField] private Sprite sPressed;

    [Header("Arrow Sprites")]
    [SerializeField] private Sprite leftReleased;
    [SerializeField] private Sprite leftPressed;
    [SerializeField] private Sprite rightReleased;
    [SerializeField] private Sprite rightPressed;

    private Sprite firstReleasedSprite;
    private Sprite firstPressedSprite;
    private Sprite secondReleasedSprite;
    private Sprite secondPressedSprite;

    private float currentHP;
    private float survivalTime;
    private bool isPlaying;
    private bool expectingFirstKey;

    private Key firstKey = Key.A;
    private Key secondKey = Key.D;
    private string firstKeyLabel = "A";
    private string secondKeyLabel = "D";

    //This is where the game resets
    private void OnEnable()
    {
        currentHP = maxHP;
        survivalTime = 0f;
        isPlaying = true;
        expectingFirstKey = true;

        UpdatePrompt();
        UpdateUI();
        ConfigureKeyImages();
    }

    //This is where the timer and Hp update
    private void Update()
    {
        if (!isPlaying)
        {
            return;
        }

        UpdateKeyImages();

        float delta = Time.deltaTime;
        float drainRate = startingDrain + survivalTime * drainIncreasePerSecond;
        float timeUntilEmpty = currentHP / drainRate;

        if (timeUntilEmpty <= delta)
        {
            survivalTime += timeUntilEmpty;
            currentHP = 0f;
            isPlaying = false;

            UpdateUI();
            uiManager.OpenGameOver(survivalTime);
            return;
        }

        survivalTime += delta;
        currentHP -= drainRate * delta;

        HandleInput();
        UpdateUI();
    }

    //Handling of inputs happen here (not using the new system, I got lazy)
    private void HandleInput()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            return;
        }

        Key requiredKey;

        if (expectingFirstKey)
        {
            requiredKey = firstKey;
        }
        else
        {
            requiredKey = secondKey;
        }

        if (keyboard[requiredKey].wasPressedThisFrame)
        {
            currentHP = Mathf.Min(currentHP + hpPerPress, maxHP);

            if (expectingFirstKey)
            {
                expectingFirstKey = false;
            }
            else
            {
                expectingFirstKey = true;
            }

            UpdatePrompt();
        }
    }

    //Handling keybinds changing
    public void SetControls(int selection)
    {
        switch (selection)
        {
            case 0:
                firstKey = Key.A;
                secondKey = Key.D;
                firstKeyLabel = "A";
                secondKeyLabel = "D";
                break;

            case 1:
                firstKey = Key.LeftArrow;
                secondKey = Key.RightArrow;
                firstKeyLabel = "LEFT ARROW";
                secondKeyLabel = "RIGHT ARROW";
                break;

            case 2:
                firstKey = Key.A;
                secondKey = Key.S;
                firstKeyLabel = "A";
                secondKeyLabel = "S";
                break;

            default:
                return;
        }

        ConfigureKeyImages();
        expectingFirstKey = true;

        if (isPlaying)
        {
            UpdatePrompt();
        }
    }

    //Changing the HP color Tint
    public void SetHPColor(float value)
    {
        hpFill.color = Color.HSVToRGB(Mathf.Clamp01(value), 1f, 1f);
    }

    //This handles the "prompt" text changing
    private void UpdatePrompt()
    {
        string nextKey;

        if (expectingFirstKey)
        {
            nextKey = firstKeyLabel;
        }
        else
        {
            nextKey = secondKeyLabel;
        }

        promptText.text = $"ALTERNATE {firstKeyLabel} & {secondKeyLabel}\nPRESS {nextKey}";
    }

    //HP bar and timer text changing is handled here
    private void UpdateUI()
    {
        hpFill.fillAmount = currentHP / maxHP;
        timerText.text = $"Time: {survivalTime:F1}s";
    }

    //Handles the keys sprites 
    private void ConfigureKeyImages()
    {
        if (firstKey == Key.LeftArrow)
        {
            firstReleasedSprite = leftReleased;
            firstPressedSprite = leftPressed;
        }
        else
        {
            firstReleasedSprite = aReleased;
            firstPressedSprite = aPressed;
        }

        switch (secondKey)
        {
            case Key.D:
                secondReleasedSprite = dReleased;
                secondPressedSprite = dPressed;
                break;

            case Key.S:
                secondReleasedSprite = sReleased;
                secondPressedSprite = sPressed;
                break;

            case Key.RightArrow:
                secondReleasedSprite = rightReleased;
                secondPressedSprite = rightPressed;
                break;
        }

        ResetKeyImages();
    }

    //Handles the sprites changing when they are pressed/released
    private void UpdateKeyImages()
    {
        Keyboard keyboard = Keyboard.current;

        if (keyboard == null)
        {
            ResetKeyImages();
            return;
        }

        if (keyboard[firstKey].isPressed)
        {
            firstKeyImage.sprite = firstPressedSprite;
        }
        else
        {
            firstKeyImage.sprite = firstReleasedSprite;
        }

        if (keyboard[secondKey].isPressed)
        {
            secondKeyImage.sprite = secondPressedSprite;
        }
        else
        {
            secondKeyImage.sprite = secondReleasedSprite;
        }
    }

    //Resets the key imagees
    private void ResetKeyImages()
    {
        firstKeyImage.sprite = firstReleasedSprite;
        secondKeyImage.sprite = secondReleasedSprite;
    }
}