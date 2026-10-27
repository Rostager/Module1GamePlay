using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMelee : MonoBehaviour
{
    [SerializeField] private InputActionReference meleeAction;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Transform batPivot;
    [SerializeField] private MeleeHitbox hitboxPrefab;

    [SerializeField] private float hitboxDistance = 1.5f;
    [SerializeField] private float hitboxLifetime = 0.15f;
    [SerializeField] private float swingDuration = 0.12f;
    [SerializeField] private float returnDuration = 0.18f;
    [SerializeField] private float aimDistance = 100f;

    private Quaternion restingRotation;
    private bool isSwinging;

    private void Awake()
    {
        restingRotation = batPivot.localRotation;
    }

    private void OnEnable()
    {
        meleeAction.action.Enable();
    }

    private void Update()
    {
        if (!isSwinging && meleeAction.action.WasPressedThisFrame())
        {
            StartCoroutine(Swing());
        }
    }

    private IEnumerator Swing()
    {
        isSwinging = true;

        Vector3 direction = playerCamera.forward;

        Vector3 aimPoint = playerCamera.position + direction * aimDistance;

        if (Physics.Raycast(playerCamera.position,direction,out RaycastHit hit,aimDistance,Physics.DefaultRaycastLayers,QueryTriggerInteraction.Ignore))
        {
            aimPoint = hit.point;
        }

        Debug.DrawLine(playerCamera.position,aimPoint,Color.green,1f);

        Vector3 hitboxPosition = playerCamera.position + direction * hitboxDistance;

        MeleeHitbox hitbox = Instantiate(hitboxPrefab,hitboxPosition,playerCamera.rotation);

        hitbox.Initialize(aimPoint);
        Destroy(hitbox.gameObject, hitboxLifetime);

        Quaternion swungRotation = restingRotation * Quaternion.Euler(0f, 0f, 110f);

        yield return RotateBat(restingRotation,swungRotation,swingDuration);

        yield return RotateBat(swungRotation,restingRotation,returnDuration);

        isSwinging = false;
    }

    private IEnumerator RotateBat(Quaternion from,Quaternion to,float duration)
    {
        float elapsed = 0f;
        duration = Mathf.Max(duration, 0.01f);

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / duration);
            t = Mathf.SmoothStep(0f, 1f, t);

            batPivot.localRotation = Quaternion.Slerp(from, to, t);

            yield return null;
        }

        batPivot.localRotation = to;
    }

    private void OnDisable()
    {
        meleeAction.action.Disable();
        StopAllCoroutines();

        batPivot.localRotation = restingRotation;
        isSwinging = false;
    }
}