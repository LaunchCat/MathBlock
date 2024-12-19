using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 1000f;
    [SerializeField] private float backwardSpeed = 5f;
    [SerializeField] private float rotationSpeed = 3f;
    public InputAction playerControls;
    Rigidbody rb;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector2 movement = playerControls.ReadValue<Vector2>();
        transform.Rotate(Vector3.up, movement.x * rotationSpeed * Time.fixedDeltaTime);
        
        Vector3 movement3D = transform.forward * movement.y;
        rb.MovePosition( transform.position + movement3D * (forwardSpeed * Time.fixedDeltaTime));
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }
    
}
