using UnityEngine;
using System.Collections.Generic;

public class ColorCollideDupe : MonoBehaviour
{
    public Material[] availableColors;
    public GameObject ballDupe;

    private Renderer[] childRenderers;

    void Awake()
    {
        childRenderers = GetComponentsInChildren<Renderer>(true);
    }

    void OnCollisionEnter(Collision collision)
    {
        Transform hit = collision.collider.transform;

        while (hit != null)
        {
            if (hit.CompareTag("Knocker"))
            {
                ChangeMaterial();
                Instantiate(ballDupe, this.transform.position, this.transform.rotation);
                return;
            }

            hit = hit.parent;
        }
    }

    void ChangeMaterial()
    {
        if (availableColors == null || availableColors.Length == 0)
            return;

        Material selected =
            availableColors[Random.Range(0, availableColors.Length)];

        if (selected == null)
            return;

        foreach (Renderer child in childRenderers)
        {
            // Replaces every material slot on each child.
            Material[] slots = child.sharedMaterials;

            for (int i = 0; i < slots.Length; i++)
                slots[i] = selected;

            child.sharedMaterials = slots;
        }
    }
}
