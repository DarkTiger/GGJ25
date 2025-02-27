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
    [SerializeField] ParticleSystem respawnParticles = null;

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

    void Start()
    {
        Respawn();
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
        GetComponent<PlayRandomSound>().PlayAudio(0, 1f);

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

    public void ResetBodyStats(float delay)
    {
        CurrentSpheresCount = 0;
        CurrentCubesCount = 0;
        CurrentPiramidsCount = 0;

        ResetBodyShapes(1f);
    }

    public void NewBodyShape()
    {
        ResetBodyShapes(1f);

        StartCoroutine(NewBodyShapeCO());
    }

    IEnumerator NewBodyShapeCO()
    {
        yield return new WaitForSeconds(3f);
        shapesParent.GetChild(GameManager.Instance.CurrentShapeIndex).gameObject.SetActive(true);
    }

    public void ResetBodyShapes(float delaySeconds)
    {
        StartCoroutine(ResetBodyShapesCO(delaySeconds));
    }

    IEnumerator ResetBodyShapesCO(float delaySeconds)
    {
        yield return new WaitForSeconds(delaySeconds);

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

    public void Respawn()
    {
        GetComponent<PlayRandomSound>().PlayAudio(1, 1f);
        transform.position = Vector3.zero;
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        respawnParticles.transform.position = Vector3.zero;
        respawnParticles.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ShapeChanger"))
        {
            Bonus_ShapeChanger shapeChanger = other.GetComponent<Bonus_ShapeChanger>();

            if (shapeChanger.ShapeIndex != GameManager.Instance.CurrentShapeData.PlayerShapeType)
            {
                if (CurrentSpheresCount == 0 && CurrentCubesCount == 0 && CurrentPiramidsCount == 0)
                {
                    ChangeShape(shapeChanger.ShapeIndex);
                }

                GameManager.Instance.Error();
            }
            else
            {
                ChangeShape(shapeChanger.ShapeIndex);
                GameManager.Instance.OnPlayerChanged(CurrentShapeIndex, CurrentSpheresCount, CurrentCubesCount, CurrentPiramidsCount);
            }

            shapeChanger.Disable();
            shapeChanger.GetComponent<PlayRandomSound>().PlayAudio(0,1f);
        }

        if (other.CompareTag("Bubble"))
        {
            Bubble bubble = other.GetComponent<Bubble>();
            ShapeType bubbleType = bubble.ShapeIndex;

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

                switch (bubble.ShapeIndex)
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

            bubble.Disable();
            bubble.GetComponent<PlayRandomSound>().PlayAudio(0,1f);
        }

        var fish = other.GetComponent<Fish>();
        if (fish)
        {
            var hitbox = fish.GetHitBox();
            if (hitbox == other)
            {
                if (CurrentSpheresCount > 0 || CurrentCubesCount > 0 || CurrentPiramidsCount > 0)
                {
                    NewBodyShape();
                }
                else
                {
                    GameManager.Instance.Scores -= 100;
                    GameManager.Instance.Scores = Mathf.Clamp(GameManager.Instance.Scores, 0, 99999);
                    HUD.Instance.SetPoints(GameManager.Instance.Scores);
                }


                Respawn();
            }
        }
    }
}
