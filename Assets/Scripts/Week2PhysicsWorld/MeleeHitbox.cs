using System.Collections.Generic;
using UnityEngine;

public class MeleeHitbox : MonoBehaviour
{
    private Vector3 aimPoint;
    private readonly HashSet<PinkOrb> hitOrbs = new();

    public void Initialize(Vector3 targetPoint)
    {
        aimPoint = targetPoint;
    }

    private void OnTriggerEnter(Collider other)
    {
        PinkOrb orb = other.GetComponentInParent<PinkOrb>();

        if (orb != null && hitOrbs.Add(orb))
        {
            Vector3 direction = aimPoint - orb.transform.position;
            orb.Deflect(direction);
        }
    }
}