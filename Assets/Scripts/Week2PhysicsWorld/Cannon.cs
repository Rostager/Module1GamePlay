using UnityEngine;

public class Cannon : MonoBehaviour
{
    [SerializeField] private Rigidbody pinkBall;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private Transform player;

    [SerializeField] private float shotDelay = 2f;
    [SerializeField] private float startDelay = 1f;
    [SerializeField] private float ballLifetime = 10f;

    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float maxSpeed = 10f;

    [SerializeField] private float minSize = 0.2f;
    [SerializeField] private float maxSize = 0.6f;

    private float nextShot;

    private void OnEnable()
    {
        nextShot = Time.time + startDelay;
    }

    private void Update()
    {
        if (Time.time >= nextShot)
        {
            Shoot();
            nextShot = Time.time + Mathf.Max(shotDelay, 0.1f);
        }
    }

    private void Shoot()
    {
        Vector3 direction = (player.position - shootPoint.position).normalized;
        Rigidbody ball = Instantiate(pinkBall, shootPoint.position, Quaternion.identity);

        float size = Random.Range(minSize, maxSize);
        float speed = Random.Range(minSpeed, maxSpeed);

        ball.transform.localScale = Vector3.one * size;
        ball.linearVelocity = direction * speed;

        Destroy(ball.gameObject, ballLifetime);
    }
}