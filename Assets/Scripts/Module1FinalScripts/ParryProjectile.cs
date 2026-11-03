using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(SphereCollider))]
public sealed class ParryProjectile : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float launchSpeed = 12f;
    [SerializeField] private float reflectedSpeed = 20f;
    [SerializeField] private float lifetime = 10f;

    private Rigidbody body;
    private SphereCollider projectileCollider;
    private bool hasBeenParried;

    private Collider shooterCollider;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        projectileCollider = GetComponent<SphereCollider>();
    }

    private void Start()
    {
        if (!hasBeenParried)
        {
            body.linearVelocity = transform.forward * launchSpeed;
        }

        Destroy(gameObject, lifetime);
    }

    public bool TryDeflect(Vector3 aimPoint, Collider playerCollider)
    {
        if (hasBeenParried)
        {
            return false;
        }

        Vector3 direction = aimPoint - body.position;

        if (direction.sqrMagnitude < 0.001f)
        {
            return false;
        }

        hasBeenParried = true;

        if (shooterCollider != null)
        {
            Physics.IgnoreCollision(projectileCollider, shooterCollider, false);
        }

        Physics.IgnoreCollision(projectileCollider, playerCollider);
        body.linearVelocity = direction.normalized * reflectedSpeed;

        return true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (hasBeenParried)
        {
            EnemyHealth enemyHealth = collision.collider.GetComponentInParent<EnemyHealth>();

            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(1);
            }
        }
        else
        {
            PlayerHealth playerHealth = collision.collider.GetComponentInParent<PlayerHealth>();

            if (playerHealth != null)
            {
                playerHealth.TakeDamage(1);
            }
        }

        Destroy(gameObject);
    }


    public void SetShooter(Collider shooter)
    {
        shooterCollider = shooter;

        Physics.IgnoreCollision(projectileCollider, shooterCollider);
    }
}