using UnityEngine;

public sealed class PlayerParry : MonoBehaviour
{
    public enum ParryState
    {
        Ready,
        Active,
        Cooldown
    }

    [Header("References")]
    [SerializeField] private PlayerInputReader input;
    [SerializeField] private SpatulaPose spatulaPose;
    [SerializeField] private BoxCollider parryHitbox;

    [Header("Timing")]
    [SerializeField, Min(0.01f)] private float activeDuration = 0.15f;
    [SerializeField, Min(0f)] private float missCooldown = 0.75f;

    [Header("Debug")]
    [SerializeField] private ParryState currentState = ParryState.Ready;

    [SerializeField] private Camera playerCamera;
    [SerializeField] private Collider playerCollider;

    [Header("Detection")]
    [SerializeField] private LayerMask projectileLayers;
    [SerializeField] private LayerMask aimLayers;
    [SerializeField] private float aimDistance = 100f;

    private float timeRemaining;

    public ParryState CurrentState => currentState;
    public float CooldownRemaining => currentState == ParryState.Cooldown ? timeRemaining : 0f;

    private void Awake()
    {
        parryHitbox.enabled = false;
    }

    private void Update()
    {
        UpdateTimer();

        if (currentState == ParryState.Ready && input.ParryPressed)
        {
            StartParry();
        }
    }




    private void FixedUpdate()
    {
        if (currentState == ParryState.Active)
        {
            CheckForProjectiles();
        }
    }



    private void CheckForProjectiles()
    {
        Transform hitboxTransform = parryHitbox.transform;

        Vector3 center = hitboxTransform.TransformPoint(parryHitbox.center);
        Vector3 halfSize = Vector3.Scale(parryHitbox.size * 0.5f, hitboxTransform.lossyScale);

        Collider[] hits = Physics.OverlapBox(center, halfSize, hitboxTransform.rotation, projectileLayers, QueryTriggerInteraction.Ignore);

        foreach (Collider hit in hits)
        {
            if (!hit.TryGetComponent(out ParryProjectile projectile))
            {
                continue;
            }

            Vector3 aimPoint = GetAimPoint();

            if (projectile.TryDeflect(aimPoint, playerCollider))
            {
                CompleteSuccessfulParry();
                return;
            }
        }
    }




    private Vector3 GetAimPoint()
    {
        Ray aimRay = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        if (Physics.Raycast(aimRay, out RaycastHit hit, aimDistance, aimLayers, QueryTriggerInteraction.Ignore))
        {
            return hit.point;
        }

        return aimRay.GetPoint(aimDistance);
    }





    private void UpdateTimer()
    {
        if (currentState == ParryState.Ready)
        {
            return;
        }

        timeRemaining = Mathf.Max(timeRemaining - Time.deltaTime, 0f);

        if (timeRemaining > 0f)
        {
            return;
        }

        if (currentState == ParryState.Active)
        {
            MissParry();
        }
        else
        {
            currentState = ParryState.Ready;
        }
    }






    private void StartParry()
    {
        currentState = ParryState.Active;
        timeRemaining = activeDuration;

        parryHitbox.enabled = true;
        spatulaPose.ShowBlock();

        CheckForProjectiles();
    }





    private void MissParry()
    {
        currentState = ParryState.Cooldown;
        timeRemaining = missCooldown;

        parryHitbox.enabled = false;
        spatulaPose.ShowRest();
    }






    public void CompleteSuccessfulParry()
    {
        if (currentState != ParryState.Active)
        {
            return;
        }

        currentState = ParryState.Ready;
        timeRemaining = 0f;

        parryHitbox.enabled = false;
        spatulaPose.ShowRest();
    }





    private void OnDisable()
    {
        currentState = ParryState.Ready;
        timeRemaining = 0f;

        parryHitbox.enabled = false;
        spatulaPose.ShowRest();
    }




}