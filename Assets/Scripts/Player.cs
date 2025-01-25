using System.Collections;
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
    [SerializeField] Vector2 playerMovementBorder = new Vector2(58.5f, 38.5f);
    [SerializeField] Transform shapesParent = null; 

    public ShapeType CurrentShapeIndex { get; private set; } = 0;
    public int CurrentSpheresCount { get; set; } = 0;
    public int CurrentCubesCount { get; set; } = 0;
    public int CurrentPiramidsCount { get; set; } = 0;

    public static Player Instance = null; 

    float baseMaxSpeed = 1f;
    float currentDashTime = 0f;
    float currentDashCooldown = 0f;
    Vector2 moveValue = Vector2.zero;
    PlayerInput playerInput = null;
    InputAction moveAction = null;
    InputAction jumpAction = null;
    Rigidbody rb = null;


    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        baseMaxSpeed = maxSpeed;
        currentDashCooldown = 0f;
    }

    private void Update()
    {
        Movement();
        Dash();  
    }

    void Movement()
    {
        moveValue = moveAction.ReadValue<Vector2>();
        moveValue = moveValue.magnitude > 0.1f ? moveValue : Vector2.zero;

        if (moveValue.magnitude > 0.1f)
        {
            rb.AddForce(moveValue.x * speed, moveValue.y * speed, 0f);
            rb.linearVelocity = Vector3.ClampMagnitude(rb.linearVelocity, maxSpeed);
        }

        transform.position = new Vector3(Mathf.Clamp(transform.position.x, -playerMovementBorder.x, playerMovementBorder.x), Mathf.Clamp(transform.position.y, -playerMovementBorder.y, playerMovementBorder.y), transform.position.z);
    }

    void Dash()
    {
        currentDashCooldown -= Time.deltaTime;

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
            rb.AddForce(moveValue.normalized * dashForce, ForceMode.Force);
            currentDashTime += Time.deltaTime;
            maxSpeed = maxSpeedDash;
            yield return null;
        }

        maxSpeed = baseMaxSpeed;
        currentDashTime = 0f;
    }

    public void ChangeShape(ShapeType shapeIndex)
    {
        for (int i = 0; i < 3; i++)
        {
            transform.GetChild(i).gameObject.SetActive((int)shapeIndex == i);
            CurrentShapeIndex = shapeIndex;
        }
    }

    public void ResetStats()
    {
        CurrentSpheresCount = 0;
        CurrentCubesCount = 0;
        CurrentPiramidsCount = 0;

        ResetBodyShapes();
    }

    public void NewBodyShape()
    {
        ResetBodyShapes();

        StartCoroutine(NewBodyShapeCO());
    }

    IEnumerator NewBodyShapeCO()
    {
        yield return null;
        shapesParent.GetChild(GameManager.Instance.CurrentShapeIndex).gameObject.SetActive(true);
    }

    public void ResetBodyShapes()
    {
        StartCoroutine(ResetBodyShapesCO());
    }

    IEnumerator ResetBodyShapesCO()
    {
        for (int i = 0; i < shapesParent.childCount; i++)
        {
            for (int j = 0; j < shapesParent.GetChild(i).childCount; j++)
            {
                shapesParent.GetChild(i).GetChild(j).gameObject.SetActive(false);
            }

            shapesParent.GetChild(i).gameObject.SetActive(false);
        }

        yield return null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShapeChanger"))
        {
            if (other.GetComponent<Bonus_ShapeChanger>().ShapeIndex != GameManager.Instance.CurrentShapeData.PlayerShapeType)
            {
                GameManager.Instance.Error();
            }
            else
            {
                ChangeShape(other.GetComponent<Bonus_ShapeChanger>().ShapeIndex);
                GameManager.Instance.OnPlayerChanged(CurrentShapeIndex, CurrentSpheresCount, CurrentCubesCount, CurrentPiramidsCount);
            }
                        
            Destroy(other.gameObject);
        }

        if (other.CompareTag("Bubble"))
        {
            ShapeType bubbleType = other.GetComponent<Bubble>().ShapeIndex;

            if (CurrentShapeIndex != GameManager.Instance.CurrentShapeData.PlayerShapeType || 
                (bubbleType == ShapeType.Sphere && CurrentSpheresCount + 1 > GameManager.Instance.CurrentShapeData.SpheresCount ||
                bubbleType == ShapeType.Cube && CurrentCubesCount + 1 > GameManager.Instance.CurrentShapeData.CubesCount ||
                bubbleType == ShapeType.Piramid && CurrentPiramidsCount + 1 > GameManager.Instance.CurrentShapeData.PiramidsCount))
            {
                GameManager.Instance.Error();
            }
            else
            {
                Transform bodyParts = shapesParent.GetChild(GameManager.Instance.CurrentShapeIndex);

                switch (other.GetComponent<Bubble>().ShapeIndex)
                {
                    case ShapeType.Sphere:
                        CurrentSpheresCount++;
                        for (int i = 0; i < bodyParts.childCount; i++)
                        {
                            if (bodyParts.GetChild(i).GetComponent<BodyPart>().ShapeIndex == ShapeType.Sphere && !bodyParts.GetChild(i).gameObject.activeSelf)
                            {
                                bodyParts.GetChild(i).gameObject.SetActive(true);
                                break;
                            }
                        }
                        break;
                    case ShapeType.Cube:
                        CurrentCubesCount++;
                        for (int i = 0; i < bodyParts.childCount; i++)
                        {
                            if (bodyParts.GetChild(i).GetComponent<BodyPart>().ShapeIndex == ShapeType.Cube && !bodyParts.GetChild(i).gameObject.activeSelf)
                            {
                                bodyParts.GetChild(i).gameObject.SetActive(true);
                                break;
                            }
                        }
                        break;
                    case ShapeType.Piramid:
                        CurrentPiramidsCount++;
                        for (int i = 0; i < bodyParts.childCount; i++)
                        {
                            if (bodyParts.GetChild(i).GetComponent<BodyPart>().ShapeIndex == ShapeType.Piramid && !bodyParts.GetChild(i).gameObject.activeSelf)
                            {
                                bodyParts.GetChild(i).gameObject.SetActive(true);
                                break;
                            }
                        }
                        break;
                }

                GameManager.Instance.OnPlayerChanged(CurrentShapeIndex, CurrentSpheresCount, CurrentCubesCount, CurrentPiramidsCount);
            }

            Destroy(other.gameObject);
        }
    }
}
