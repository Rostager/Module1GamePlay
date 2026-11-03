using UnityEngine;

public sealed class EnemyShooter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;
    [SerializeField] private Transform firePoint;
    [SerializeField] private ParryProjectile projectilePrefab;
    [SerializeField] private Collider enemyCollider;

    [Header("Shooting")]
    [SerializeField, Min(0.1f)] private float shotInterval = 2f;
    [SerializeField, Min(0f)] private float initialDelay = 1f;

    private float shotTimer;

    private void OnEnable()
    {
        shotTimer = initialDelay;
    }

    private void Update()
    {
        if (target == null)
        {
            return;
        }

        shotTimer -= Time.deltaTime;

        if (shotTimer <= 0f)
        {
            Fire();
            shotTimer = shotInterval;
        }
    }

    private void Fire()
    {
        Vector3 direction = target.position - firePoint.position;

        if (direction.sqrMagnitude < 0.001f)
        {
            return;
        }

        Quaternion rotation = Quaternion.LookRotation(direction);

        ParryProjectile projectile = Instantiate(projectilePrefab, firePoint.position, rotation);
        projectile.SetShooter(enemyCollider);
    }
}