using System;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Knocker : MonoBehaviour
{
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float turnSpeed = 120f;
    [SerializeField] float scaleSpeed = 0.05f;
    [SerializeField] float maxScaleSize = 3f;
    private float scaleSize;
    public InputActionReference moveAction;
    public InputActionReference rotateAction;
    public InputActionReference scaleAction;

    private Rigidbody rb;

    void OnEnable()
    {
        moveAction.action.Enable();
        rotateAction.action.Enable();
        scaleAction.action.Enable();
   
    }

    void OnDisable()
    {
        moveAction.action.Disable();
        rotateAction.action.Disable();
        scaleAction.action.Disable();
        
    }
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        scaleSize = this.transform.localScale.y;
        
    }

    void FixedUpdate()
    {
        Vector2 inputDirection = moveAction.action.ReadValue<Vector2>();
        UnityEngine.Debug.Log(inputDirection);
        this.rb.linearVelocity = new Vector3(inputDirection.x * moveSpeed, 0, inputDirection.y * moveSpeed);
    }

    private void Update()
    {
        UnityEngine.Debug.Log(rotateAction.action.IsPressed());
        if (rotateAction.action.IsPressed())
        {
            rb.angularVelocity = Vector3.up * 180f * Mathf.Deg2Rad;
        }
        else
        {
            this.rb.angularVelocity = new Vector3(0,0,0);
        }

        if (scaleAction.action.IsPressed())
        {
            scaleSize = scaleSize + scaleSpeed;
            
        }
        this.transform.localScale = new Vector3(1, MathF.Cos(scaleSize) * maxScaleSize, 1);
    }
}
