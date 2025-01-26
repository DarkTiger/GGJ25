using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    [SerializeField] int gameTime = 300;

    public ShapeData[] ShapesData;

    public int Scores { get; set; } = 0;
    
    public static GameManager Instance = null;
    public float GameTime => currentGameTime;
    public ShapeData CurrentShapeData { get; private set; }
    public int CurrentShapeIndex = 0;

    bool gameRunning = true;

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
        StartCoroutine(NewShape());
    }

    private void Update()
    {
        if (currentGameTime > 0)
        {
            currentGameTime -= Time.deltaTime;
        }
        else if (gameRunning)
        {
            gameRunning = false;
            GameOver();
        }
    }
    
    public IEnumerator NewShape()
    {
        yield return new WaitForSeconds(1f);

        int newShapeIndex = Random.Range(0, ShapesData.Length);
        CurrentShapeIndex = newShapeIndex;
        CurrentShapeData = ShapesData[newShapeIndex];
        HUD.Instance.SetShapeSprite(CurrentShapeData.Sprite);

        Player.Instance.NewBodyShape();
    }

    void ShapeFinish()
    {
        Player.Instance.ResetBodyStats(3f);
        Scores += 150;
        HUD.Instance.SetPoints(Scores);
        GetComponent<PlayRandomSound>().PlayAudio(0, 1f);

        StartCoroutine(NewShape());
    }

    public void Error()
    {
        GetComponent<PlayRandomSound>().PlayAudio(1, 1f);
        Scores -= 50;
        Scores = Mathf.Clamp(Scores, 0, 99999);
        HUD.Instance.SetPoints(Scores);
    }

    public void GameOver()
    {
        GetComponent<AudioSource>().Stop();
        GetComponent<PlayRandomSound>().PlayAudio(2, 1f);
        HUD.Instance.ActiveGameOver();
    }

    public void OnPlayerChanged(ShapeType playerShapeIndex, int currentPlayerSphere, int currentPlayerCubes, int currentPlayerPiramid)
    {
        if (CurrentShapeData.PlayerShapeType == playerShapeIndex &&
            CurrentShapeData.SpheresCount == currentPlayerSphere &&
            CurrentShapeData.CubesCount == currentPlayerCubes &&
            CurrentShapeData.PiramidsCount == currentPlayerPiramid)
        {
            ShapeFinish();
        }
    }
}
