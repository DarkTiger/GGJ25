using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTime = null;
    [SerializeField] TextMeshProUGUI txtPoints = null;
    [SerializeField] Image imgShape = null;
    [SerializeField] GameObject gameOver = null;

    public static HUD Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        TimeSpan timeSpam = TimeSpan.FromSeconds(GameManager.Instance.GameTime);
        txtTime.text = Mathf.Clamp(timeSpam.Minutes, 0, 60).ToString("00") + ":" + Mathf.Clamp(timeSpam.Seconds, 0, 60).ToString("00");
    }

    public void SetShapeSprite(Sprite sprite)
    {
        imgShape.sprite = sprite;
    }

    public void SetPoints(int points)
    {
        txtPoints.text = "Points: " + points;
    }

    public void ActiveGameOver(bool enable)
    {
        gameOver.SetActive(enable);
    }
}