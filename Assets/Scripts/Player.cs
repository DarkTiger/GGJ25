using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float maxSpeed = 5f;
    [SerializeField] float maxSpeedDash = 10f;
    [SerializeField] float dashForce = 5f;
    [SerializeField] float dashTime = 0.25f;
    [SerializeField] float dashCooldown = 5f;
    float baseMaxSpeed = 1f;
    float currentDashTime = 0f;
    float currentDashCooldown = 0f;

    PlayerInput playerInput = null;
    InputAction moveAction = null;
    InputAction jumpAction = null;
    Rigidbody rb = null;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        baseMaxSpeed = maxSpeed;
        currentDashCooldown = 0f;
    }

    private void Update()
    {
        Vector2 moveValue = moveAction.ReadValue<Vector2>();

        if (moveValue.magnitude > 0.1f)
        {
            rb.AddForce(moveValue.x * speed, moveValue.y * speed, 0f);
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }

        currentDashCooldown -= Time.deltaTime;

        Dash();
    }

    void Dash()
    {
        if (currentDashCooldown > 0f) return;

        if (jumpAction.WasPerformedThisFrame())
        {
            StartCoroutine(DashCO());
        }
    }

    IEnumerator DashCO()
    {
        currentDashCooldown = dashCooldown;

        while (currentDashTime < dashTime)
        {
            rb.AddForce(rb.linearVelocity.normalized * dashForce, ForceMode.Force);
            currentDashTime += Time.deltaTime;
            maxSpeed = maxSpeedDash;
            yield return null;
        }

        maxSpeed = baseMaxSpeed;
        currentDashTime = 0f;
    }
}
