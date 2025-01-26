using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTime = null;
    [SerializeField] TextMeshProUGUI txtPoints = null;
    [SerializeField] Image imgShape = null;
    [SerializeField] GameObject gameOver = null;
    [SerializeField] GameObject result = null;
    [SerializeField] TextMeshProUGUI resultScore = null;

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

    public void ActiveGameOver()
    {
        StartCoroutine(GameOverCO());
    }

    IEnumerator GameOverCO()
    {
        gameOver.SetActive(true);

        yield return new WaitForSeconds(5f);

        gameOver.SetActive(false);
        result.SetActive(true);

        resultScore.text = "SCORES: " + GameManager.Instance.Scores;

        yield return new WaitForSeconds(5f);

        SceneManager.LoadScene(0);
    }
}