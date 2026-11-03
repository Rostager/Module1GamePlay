using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public sealed class EnemyOrbitMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform target;

    [Header("Orbit Distance")]
    [SerializeField] private float minimumDistance = 7f;
    [SerializeField] private float maximumDistance = 11f;
    [SerializeField] private float distanceCorrectionStrength = 1.5f;

    [Header("Movement")]
    [SerializeField] private float movementSpeed = 3f;
    [SerializeField] private float verticalSpeed = 2f;

    [Header("Flying Height")]
    [SerializeField] private float minimumHeight = 3f;
    [SerializeField] private float maximumHeight = 5f;

    [Header("Distance Change Timing")]
    [SerializeField] private float minimumChangeTime = 2f;
    [SerializeField] private float maximumChangeTime = 4f;

    private Rigidbody body;

    private float preferredDistance;
    private float flyingHeight;
    private float orbitDirection;
    private float distanceChangeTimer;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        flyingHeight = Random.Range(minimumHeight, maximumHeight);
        orbitDirection = Random.value < 0.5f ? -1f : 1f;

        ChooseOrbitDistance();
    }

    private void FixedUpdate()
    {
        if (target == null)
        {
            return;
        }

        UpdateDistanceTimer();
        MoveAroundTarget();
    }

    private void UpdateDistanceTimer()
    {
        distanceChangeTimer -= Time.fixedDeltaTime;

        if (distanceChangeTimer <= 0f)
        {
            ChooseOrbitDistance();
        }
    }

    private void ChooseOrbitDistance()
    {
        preferredDistance = Random.Range(minimumDistance, maximumDistance);
        distanceChangeTimer = Random.Range(minimumChangeTime, maximumChangeTime);
    }

    private void MoveAroundTarget()
    {
        Vector3 offset = body.position - target.position;
        offset.y = 0f;

        float currentDistance = offset.magnitude;

        Vector3 outwardDirection = Vector3.forward;

        if (currentDistance > 0.01f)
        {
            outwardDirection = offset / currentDistance;
        }

        Vector3 orbitVelocity = Vector3.Cross(Vector3.up, outwardDirection);
        orbitVelocity *= orbitDirection * movementSpeed;

        float distanceError = preferredDistance - currentDistance;
        Vector3 correctionVelocity = outwardDirection * distanceError * distanceCorrectionStrength;

        Vector3 horizontalVelocity = orbitVelocity + correctionVelocity;
        horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, movementSpeed);

        Vector3 nextPosition = body.position + horizontalVelocity * Time.fixedDeltaTime;
        nextPosition.y = Mathf.MoveTowards(body.position.y, flyingHeight, verticalSpeed * Time.fixedDeltaTime);

        body.MovePosition(nextPosition);
    }


    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }
}