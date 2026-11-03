using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class PlayerHealth : MonoBehaviour
{
    [Header("Health")]
    [SerializeField, Min(1)] private int maximumHealth = 3;
    [SerializeField, Min(0f)] private float invulnerabilityDuration = 0.75f;

    [Header("References")]
    [SerializeField] private PlayerStateMachine movement;
    [SerializeField] private PlayerLook look;
    [SerializeField] private PlayerParry parry;
    [SerializeField] private PlayerInputReader input;

    [Header("Debug")]
    [SerializeField] private int currentHealth;
    [SerializeField] private bool isDead;

    private Rigidbody body;
    private float nextDamageTime;

    public int CurrentHealth => currentHealth;
    public int MaximumHealth => maximumHealth;
    public bool IsDead => isDead;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        currentHealth = maximumHealth;
    }

    public void TakeDamage(int damage)
    {
        if (isDead || damage <= 0)
        {
            return;
        }

        if (Time.time < nextDamageTime)
        {
            return;
        }

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        nextDamageTime = Time.time + invulnerabilityDuration;

        Debug.Log($"Player health: {currentHealth}/{maximumHealth}");

        if (currentHealth == 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;

        movement.enabled = false;
        parry.enabled = false;
        look.enabled = false;
        input.enabled = false;

        body.linearVelocity = Vector3.zero;
        body.angularVelocity = Vector3.zero;
        body.isKinematic = true;

        Debug.Log("Game over.");
    }
}