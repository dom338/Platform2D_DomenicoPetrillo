using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    private string gameSceneName = "Level_1";
    private string CreditsSceneName = "Credits_Scene";
    private string ManiMenuSceneName = "Main_Menu_Scene";

    public void PlayGame()
    {
        SceneManager.LoadSceneAsync(gameSceneName);
    }

    public void OpenCredits()
    {
        SceneManager.LoadSceneAsync(CreditsSceneName);
    }

    public void QuitGame()
    {
        Debug.Log("exit_game");
        Application.Quit();
    }

    public void BacktoMenu()
    {
        SceneManager.LoadScene(ManiMenuSceneName);
    }
}
