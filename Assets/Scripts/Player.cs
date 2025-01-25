using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float maxSpeed = 5f;
    Rigidbody rb = null;
    PlayerInput playerInput = null;
    InputAction moveAction = null;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
    }

    private void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>() * speed;
        rb.AddForce(moveValue.x, 0f, moveValue.y);
        rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
    }
}
