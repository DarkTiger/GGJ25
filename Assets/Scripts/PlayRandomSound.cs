using UnityEngine;

public class PlayRandomSound : MonoBehaviour
{
    [SerializeField] AudioClip[] clips1;
    [SerializeField] AudioClip[] clips2;
    [SerializeField] AudioClip[] clips3;

    public void PlayAudio(int clipsPoolIndex, float volume)
    {
        switch (clipsPoolIndex)
        {
            case 0:
                AudioSource.PlayClipAtPoint(clips1[Random.Range(0, clips1.Length)], Camera.main.transform.position, volume);
                break;
            case 1:
                AudioSource.PlayClipAtPoint(clips2[Random.Range(0, clips2.Length)], Camera.main.transform.position, volume);
                break;
            case 2:
                AudioSource.PlayClipAtPoint(clips3[Random.Range(0, clips3.Length)], Camera.main.transform.position, volume);
                break;
        }
        
    }
}
