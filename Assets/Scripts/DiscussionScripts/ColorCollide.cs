using UnityEngine;
using System.Collections.Generic;

public class ColorCollide : MonoBehaviour
{
    public Material[] availableColors;

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
            Material[] slots = child.sharedMaterials;

            for (int i = 0; i < slots.Length; i++)
                slots[i] = selected;

            child.sharedMaterials = slots;
        }
    }
}
