using UnityEngine;


public class MainMenu : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.pointsCounter = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
