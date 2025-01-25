using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] int gameTime = 300;

    public static GameManager Instance = null;
    public float GameTime => currentGameTime;

    float currentGameTime = 0;


    private void Awake()
    {
        Instance = this;
        currentGameTime = gameTime;
    }

    private void Update()
    {
        currentGameTime -= Time.deltaTime;
    }
    
}
