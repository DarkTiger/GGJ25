using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] int gameTime = 300;

    public ShapeData[] ShapesData;

    public int Scores { get; private set; } = 0;
    
    public static GameManager Instance = null;
    public float GameTime => currentGameTime;
    public ShapeData CurrentShapeData { get; private set; }

    [Serializable]
    public struct ShapeData
    {
        public Sprite Sprite;
        public ShapeType PlayerShapeType;
        public int SpheresCount;
        public int CubesCount;
        public int PiramidsCount;
    }

    float currentGameTime = 0;


    private void Awake()
    {
        Instance = this;
        currentGameTime = gameTime;
    }

    private void Start()
    {
        Scores = 0;
        NewShape();
    }

    private void Update()
    {
        currentGameTime -= Time.deltaTime;
    }
    
    public void NewShape()
    {
        int newShapeIndex = Random.Range(0, ShapesData.Length);
        CurrentShapeData = ShapesData[newShapeIndex];
        HUD.Instance.SetShapeSprite(CurrentShapeData.Sprite);
    }

    void ShapeFinish()
    {
        Scores += 150;
        HUD.Instance.SetPoints(Scores);

        NewShape();
    }

    public void OnPlayerChanged(ShapeType playerShapeIndex, int currentPlayerSphere, int currentPlayerCubes, int currentPlayerPiramid)
    {
        Debug.Log(playerShapeIndex.ToString() + " sphere: " + currentPlayerSphere + " cube: " + currentPlayerCubes + " piramid: " + currentPlayerPiramid);

        if (CurrentShapeData.PlayerShapeType == playerShapeIndex && 
            CurrentShapeData.SpheresCount == currentPlayerSphere &&
            CurrentShapeData.CubesCount == currentPlayerCubes &&
            CurrentShapeData.PiramidsCount == currentPlayerPiramid)
        {
            ShapeFinish();
        }
    }
}
