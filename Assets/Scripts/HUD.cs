using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTime = null;
    [SerializeField] TextMeshProUGUI txtPoints = null;
    [SerializeField] Image imgShape = null;

    public static HUD Instance = null;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        TimeSpan timeSpam = TimeSpan.FromSeconds(GameManager.Instance.GameTime);
        txtTime.text = timeSpam.Minutes.ToString("00") + ":" + timeSpam.Seconds.ToString("00");
    }

    public void SetShapeSprite(Sprite sprite)
    {
        imgShape.sprite = sprite;
    }

    public void SetPoints(int points)
    {
        txtPoints.text = "Points: " + points;
    }
}