using UnityEngine;

public enum ShapeType { Sphere = 0, Cube = 1, Piramid = 2 }

public class Bonus_ShapeChanger : MonoBehaviour
{
    public ShapeType ShapeIndex = ShapeType.Sphere;
}
