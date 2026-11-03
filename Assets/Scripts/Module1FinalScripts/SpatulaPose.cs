using UnityEngine;

public sealed class SpatulaPose : MonoBehaviour
{
    [SerializeField] private Transform blockPose;

    private Vector3 restPosition;
    private Quaternion restRotation;

    private void Awake()
    {
        restPosition = transform.localPosition;
        restRotation = transform.localRotation;
    }

    public void ShowBlock()
    {
        transform.localPosition = blockPose.localPosition;
        transform.localRotation = blockPose.localRotation;
    }

    public void ShowRest()
    {
        transform.localPosition = restPosition;
        transform.localRotation = restRotation;
    }
}