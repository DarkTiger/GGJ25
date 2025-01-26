using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainManu : MonoBehaviour
{
    [SerializeField] GameObject Intro;
    [SerializeField] GameObject MainMenu;
    [SerializeField] GameObject Tutorial;
    [SerializeField] GameObject Credits;


    private IEnumerator Start()
    {
        yield return new WaitForSeconds(5);

        Intro.SetActive(false);
        MainMenu.SetActive(true);
    }

    public void GO()
    {
        SceneManager.LoadSceneAsync(1);
    }

    public void IntroEnable(bool enable)
    {
        Intro.SetActive(enable);
    }

    public void MainMenuEnable(bool enable)
    {
        MainMenu.SetActive(enable);
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
