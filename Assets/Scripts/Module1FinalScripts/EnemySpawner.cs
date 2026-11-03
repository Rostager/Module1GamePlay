using System.Collections.Generic;
using UnityEngine;

public sealed class EnemySpawner : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;
    [SerializeField] private PlayerHealth playerHealth;
    [SerializeField] private EnemyHealth enemyPrefab;

    [Header("Spawn Position")]
    [SerializeField] private float minimumSpawnDistance = 12f;
    [SerializeField] private float maximumSpawnDistance = 16f;
    [SerializeField] private float spawnHeight = 4f;

    [Header("Spawn Timing")]
    [SerializeField] private float firstSpawnDelay = 1f;
    [SerializeField] private float startingSpawnInterval = 5f;
    [SerializeField] private float minimumSpawnInterval = 1.5f;
    [SerializeField] private int maximumEnemies = 12;

    [Header("Enemy Shooting")]
    [SerializeField] private float startingShotInterval = 2f;
    [SerializeField] private float minimumShotInterval = 0.6f;

    [Header("Difficulty")]
    [SerializeField] private float timeToMaximumDifficulty = 180f;

    private readonly List<EnemyHealth> activeEnemies = new List<EnemyHealth>();

    private float survivalTime;
    private float spawnTimer;

    public float SurvivalTime => survivalTime;
    public int ActiveEnemyCount => activeEnemies.Count;

    private void Start()
    {
        spawnTimer = firstSpawnDelay;
    }

    private void Update()
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        RemoveDestroyedEnemies();

        if (playerHealth.IsDead)
        {
            return;
        }

        survivalTime += Time.deltaTime;
        spawnTimer = Mathf.Max(spawnTimer - Time.deltaTime, 0f);

        if (activeEnemies.Count >= maximumEnemies)
        {
            return;
        }

        if (spawnTimer <= 0f)
        {
            SpawnEnemy();
        }
    }

    private void RemoveDestroyedEnemies()
    {
        for (int i = activeEnemies.Count - 1; i >= 0; i--)
        {
            if (activeEnemies[i] == null)
            {
                activeEnemies.RemoveAt(i);
            }
        }
    }

    private void SpawnEnemy()
    {
        float difficulty = GetDifficulty();

        float firingInterval = Mathf.Lerp(startingShotInterval, minimumShotInterval, difficulty);
        float nextSpawnInterval = Mathf.Lerp(startingSpawnInterval, minimumSpawnInterval, difficulty);

        Vector3 spawnPosition = GetSpawnPosition();

        EnemyHealth enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);

        EnemyOrbitMovement movement = enemy.GetComponent<EnemyOrbitMovement>();
        EnemyShooter shooter = enemy.GetComponent<EnemyShooter>();

        movement.SetTarget(player);
        shooter.Initialize(player, firingInterval);

        activeEnemies.Add(enemy);
        spawnTimer = nextSpawnInterval;
    }

    private float GetDifficulty()
    {
        float duration = Mathf.Max(timeToMaximumDifficulty, 0.1f);

        return Mathf.Clamp01(survivalTime / duration);
    }

    private Vector3 GetSpawnPosition()
    {
        float angle = Random.Range(0f, Mathf.PI * 2f);
        float distance = Random.Range(minimumSpawnDistance, maximumSpawnDistance);

        Vector3 direction = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle));
        Vector3 position = player.position + direction * distance;

        position.y = spawnHeight;

        return position;
    }
}