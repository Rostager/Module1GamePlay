using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Toggler : MonoBehaviour
{
    [SerializeField] public GameObject toBeToggled;
    public InputActionReference actionToggle;
    private bool toggled = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        toBeToggled.SetActive(toggled);
        if (actionToggle.action.WasPressedThisFrame())
        {
            toggled = !toggled;
        }
    }
}
