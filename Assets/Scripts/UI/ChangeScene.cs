using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    [SerializeField] int sceneIndex = 0;


    public void OnChangeScene()
    {
        SceneManager.LoadScene(sceneIndex);
    }
}
