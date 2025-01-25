using UnityEngine;
using TMPro;
using System;

public class HUD : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI txtTime = null;

    private void Update()
    {
        TimeSpan timeSpam = TimeSpan.FromSeconds(GameManager.Instance.GameTime);
        txtTime.text = timeSpam.Minutes + ":" + timeSpam.Seconds;
    }
}