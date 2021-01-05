using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class PauseMenu : MonoBehaviour
{
    private bool GameIsPaused = false;
    [SerializeField] private GameObject mainElements = null;
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Button optionsButton;


    private void Awake()
    {
        Assert.IsNotNull(mainElements);
        mainElements.gameObject.SetActive(false);
        Assert.IsNotNull(resumeButton);
        resumeButton.onClick.AddListener(PauseOrResume);
        Assert.IsNotNull(returnToMainMenuButton);
        returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        Assert.IsNotNull(optionsButton);
        optionsButton.onClick.AddListener(Options);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            PauseOrResume();
        }
    }

    private void PauseOrResume()
    {
        if (GameIsPaused)
            Resume(true);
        else
            Pause();

        SceneManager.Instance?.Player?.SwitchCamera();
        mainElements.gameObject.SetActive(GameIsPaused);
    }

    private void Pause()
    {
        Cursor.lockState = CursorLockMode.None;
        Time.timeScale = 0.0f;
        GameIsPaused = true;
    }

    private void Resume(bool ShouldLock)
    {
        if(ShouldLock)
            Cursor.lockState = CursorLockMode.Locked;
        Time.timeScale = 1.0f;
        GameIsPaused = false;
    }

    private void ReturnToMainMenu()
    {
        Resume(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    //TODO: Replace with showing options menu.
    private void Options()
    {
        Resume(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
