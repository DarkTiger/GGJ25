using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    [SerializeField] float cameraHeight = 10f;
    [SerializeField] Vector2 mapSize = new Vector2(12f, 10f);
    [SerializeField] Vector2 bordersSize = new Vector2(5f, 5f);
    [SerializeField] Transform player = null;


    private void FixedUpdate()
    {
        Vector3 playerDirection = (player.position - transform.position);
        Vector3 cameraTarget = new Vector3(transform.position.x, cameraHeight, transform.position.z);

        if ((transform.position.x > -(mapSize.x / 2f) + bordersSize.x || playerDirection.x > 0f) && 
            (transform.position.x < (mapSize.x / 2f) - bordersSize.x || playerDirection.x < 0f))
        {
            cameraTarget.x = player.position.x;
        }
                
        if ((transform.position.z > -(mapSize.y / 2f) + bordersSize.y || playerDirection.z > 0f) &&
            (transform.position.z < (mapSize.y / 2f) - bordersSize.y || playerDirection.z < 0f))
        {
            cameraTarget.z = player.position.z;
        }

        transform.position = Vector3.Lerp(transform.position, new Vector3(cameraTarget.x, cameraHeight, cameraTarget.z), 2.5f * Time.deltaTime);
    }
}