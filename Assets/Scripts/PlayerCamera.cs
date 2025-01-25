using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float cameraDistance = 50f;
    [SerializeField] Vector2 mapSize = new Vector2(12f, 10f);
    [SerializeField] Vector2 bordersSize = new Vector2(5f, 5f);
    [SerializeField] Transform player = null;


    private void FixedUpdate()
    {
        Vector3 playerDirection = (player.position - transform.position);
        Vector3 cameraTarget = transform.position;

        if ((transform.position.x > -(mapSize.x / 2f) + bordersSize.x || playerDirection.x > 0f) && 
            (transform.position.x < (mapSize.x / 2f) - bordersSize.x || playerDirection.x < 0f))
        {
            cameraTarget.x = player.position.x;
        }
                
        if ((transform.position.y > -(mapSize.y / 2f) + bordersSize.y || playerDirection.y > 0f) &&
            (transform.position.y < (mapSize.y / 2f) - bordersSize.y || playerDirection.y < 0f))
        {
            cameraTarget.y = player.position.y;
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(cameraTarget.x, cameraTarget.y, -cameraDistance), 2.5f * Time.deltaTime);
    }
}