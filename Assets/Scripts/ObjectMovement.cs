using UnityEngine;

public class ObjectMovement : MonoBehaviour
{
    [SerializeField] Vector3 direction = Vector3.forward;
    [SerializeField] float speed = 1f;

    Vector3 startPos = Vector3.zero;
    bool returning = false;

    private void Awake()
    {
        startPos = transform.position;
    }

    void Update()
    {
        if ((transform.position - startPos).magnitude < 5f)
        {
            transform.position += direction * 1f;
        }
        else if (!returning)
        {
            returning = true;
        }
    }
}
