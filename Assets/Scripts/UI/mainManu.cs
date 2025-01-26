using UnityEngine;
using UnityEngine.SceneManagement;

public class mainManu : MonoBehaviour
{
   public void GO()
    {
        SceneManager.LoadSceneAsync(1);

    }
    public void quitgame()
    {
        Application.Quit();
    }
}
