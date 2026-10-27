using UnityEngine;

public class PinkOrb : MonoBehaviour
{
    [SerializeField] private float deflectSpeed = 15f;

    public bool WasDeflected { get; private set; }

    public void Deflect(Vector3 direction)
    {
        Rigidbody rb = GetComponent<Rigidbody>();

        rb.linearVelocity = direction.normalized * deflectSpeed;
        WasDeflected = true;
    }
}