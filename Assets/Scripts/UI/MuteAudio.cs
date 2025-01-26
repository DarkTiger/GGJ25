using UnityEngine;
using UnityEngine.UI;

public class MuteAudio : MonoBehaviour
{
    [SerializeField] Sprite normal;
    [SerializeField] Sprite muted;

    Image btnImage;
    bool isMuted = false;

    private void Start()
    {
        btnImage = GetComponent<Image>();
        btnImage.sprite = normal;
    }

    public void OnMuteAudio()
    {
        isMuted = !isMuted;
        AudioListener.volume = isMuted ? 0f : 1f;
        btnImage.sprite = isMuted? muted : normal;
    }
}
