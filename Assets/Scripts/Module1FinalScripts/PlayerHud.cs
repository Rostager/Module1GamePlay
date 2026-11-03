using TMPro;
using UnityEngine;

public sealed class PlayerHud : MonoBehaviour
{
    [Header("Gameplay References")]
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private PlayerParry playerParry;

    [Header("Text References")]
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text parryText;

    private void Update()
    {
        UpdateHealthText();
        UpdateParryText();
    }

    private void UpdateHealthText()
    {
        if (playerHealth.IsDead)
        {
            healthText.text = "Health: 0 — Game Over";
            return;
        }

        healthText.text = $"Health: {playerHealth.CurrentHealth} / {playerHealth.MaximumHealth}";
    }

    private void UpdateParryText()
    {
        if (playerHealth.IsDead)
        {
            parryText.text = "Parry: Unavailable";
            return;
        }

        switch (playerParry.CurrentState)
        {
            case PlayerParry.ParryState.Ready:
                parryText.text = "Parry: Ready";
                break;

            case PlayerParry.ParryState.Active:
                parryText.text = "Parry: Blocking";
                break;

            case PlayerParry.ParryState.Cooldown:
                parryText.text = $"Parry: {playerParry.CooldownRemaining:0.0}s";
                break;
        }
    }
}