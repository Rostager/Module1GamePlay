using UnityEngine;

public class WallTarget : MonoBehaviour
{
    [SerializeField] private BoxCollider wallCollider;
    [SerializeField] private float edgeMargin = 0.6f;

    private float fixedLocalX;
    private int lastHitFrame = -1;

    private void Start()
    {
       
        fixedLocalX = wallCollider.transform.InverseTransformPoint(transform.position).x;
    }

    private void OnTriggerEnter(Collider other)
    {
        PinkOrb orb = other.GetComponentInParent<PinkOrb>();

        if (orb == null || !orb.WasDeflected)
            return;

        orb.gameObject.SetActive(false);
        Destroy(orb.gameObject);

        
        if (lastHitFrame == Time.frameCount)
            return;

        lastHitFrame = Time.frameCount;
        MoveToRandomPosition();
    }

    private void MoveToRandomPosition()
    {
        Transform wall = wallCollider.transform;

        Vector3 center = wallCollider.center;
        Vector3 halfSize = wallCollider.size * 0.5f;
        Vector3 scale = wall.lossyScale;

        float marginY = edgeMargin / Mathf.Abs(scale.y);
        float marginZ = edgeMargin / Mathf.Abs(scale.z);

        float rangeY = Mathf.Max(0f, halfSize.y - marginY);
        float rangeZ = Mathf.Max(0f, halfSize.z - marginZ);

        Vector3 localPosition = new Vector3(fixedLocalX,center.y + Random.Range(-rangeY, rangeY),center.z + Random.Range(-rangeZ, rangeZ));

        transform.position = wall.TransformPoint(localPosition);
    }
}