using UnityEngine;
using UnityEngine.SceneManagement;

public class mainManu : MonoBehaviour
{
    [SerializeField] GameObject Tutorial;
    [SerializeField] GameObject Credits;

   public void GO()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void TutorialEnable(bool enable)
    {
        Tutorial.SetActive(enable);
    }

    public void CreditsEnable(bool enable)
    {
        Credits.SetActive(enable);
    }

    public void quitgame()
    {
        Application.Quit();
    }
}
