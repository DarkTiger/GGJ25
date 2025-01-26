using System.Collections;
using UnityEngine;

public enum ShapeType { Sphere = 0, Cube = 1, Piramid = 2 }

public class Bonus_ShapeChanger : MonoBehaviour
{
    [SerializeField] float respawnTime = 5f;

    public ShapeType ShapeIndex = ShapeType.Sphere;

    Renderer renderer = null;
    Collider collider = null;

    private void Awake()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
    }

    public void Disable()
    {
        renderer.enabled = false;
        collider.enabled = false;

        StartCoroutine(RespawnCO());
    }

    IEnumerator RespawnCO()
    {
        yield return new WaitForSeconds(respawnTime);

        renderer.enabled = true;
        collider.enabled = true;
    }
}
