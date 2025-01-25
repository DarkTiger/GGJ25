using UnityEngine;

public class Fish : MonoBehaviour
{
    [Header("Base Fish Settings")]
    public float Speed = 1f;
    public AnimationCurve MovementPattern = AnimationCurve.Linear(0f, 0f, 0f, 0f);

    /// <summary>
    /// How much should the fish move according to the pattern
    /// </summary>
    public float PatternIntensity = 5.0f;

    public float PatternDurationTime = 1.0f;

    private float currentLifetime = 0f;

    protected virtual void Move() {
        float patternTime = currentLifetime % PatternDurationTime;
        float patternValue = MovementPattern.Evaluate(patternTime) * PatternIntensity;
        Vector3 movement = new Vector3(0f, patternValue, 0f);
        transform.position += movement * Speed * Time.deltaTime;
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentLifetime += Time.deltaTime;
        Move();
    }
}
