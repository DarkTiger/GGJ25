using UnityEngine;

public class PlaySound : MonoBehaviour
{
    [SerializeField] AudioClip audioClip = null;
    [SerializeField] float volume = 1.0f;

    public void OnPlaySound()
    {
        AudioSource.PlayClipAtPoint(audioClip, Camera.main.transform.position, volume);
    }
}
