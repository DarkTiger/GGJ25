using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Base Fish Settings")]
    public float Speed = 1f;
    public AnimationCurve MovementPattern = AnimationCurve.Linear(0f, 0f, 0f, 0f);

    public float RightMovementIntensity = 3.0f;

    /// <summary>
    /// How much should the fish move according to the pattern
    /// </summary>
    public float PatternIntensity = 5.0f;

    public float PatternDurationTime = 1.0f;
    public bool ShouldRotateAlongPath = true;
    public float RotationIntensity = 1.0f;

    private float currentLifetime = 0f;
    private Vector3 initialPosition = Vector3.zero;


    protected virtual void Move() {
        float patternTime = currentLifetime % PatternDurationTime;
        patternTime /= PatternDurationTime;
        float patternValue = MovementPattern.Evaluate(patternTime) * PatternIntensity;
        Vector3 movement = new Vector3(0f, patternValue, 0f);
        Vector3 desiredPosition = new Vector3(transform.position.x, initialPosition.y + movement.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Speed * Time.deltaTime);

        // move horizontally
        transform.position += new Vector3(RightMovementIntensity * Speed * Time.deltaTime, 0f, 0f);
    }

/// <summary>
/// Rotates the fish face to the direction it is moving exagerating the rotation according to a RotationIntensity
/// </summary>
    void RotateAlongPath()
    {
        Vector3 direction = (transform.position - initialPosition).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, angle * RotationIntensity);
    }

    void Start()
    {
        initialPosition = transform.position;
        
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Move();
        if (ShouldRotateAlongPath){
            RotateAlongPath();
        }
        currentLifetime += Time.deltaTime;
    }
}
