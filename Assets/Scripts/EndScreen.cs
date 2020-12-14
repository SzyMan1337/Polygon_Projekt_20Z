using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;


public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainElements = null;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite vicotryCard;
    [SerializeField] private Sprite deathCard;


    private void Awake()
    {
        Assert.IsNotNull(mainElements);
        mainElements.gameObject.SetActive(false);
        Assert.IsNotNull(restartButton);
        restartButton.onClick.AddListener(Restart);
        Assert.IsNotNull(returnToMainMenuButton);
        returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        Assert.IsNotNull(cardImage);
        Assert.IsNotNull(vicotryCard);
        Assert.IsNotNull(deathCard);
    }

    private void Start()
    {
        WaveManager.OnGameWon += OnVictory;
        SceneManager.Instance.Player.Health.OnDeath += OnPlayerDeath;
    }

    private void OnDestroy()
    {
        WaveManager.OnGameWon -= OnVictory;
    }

    private void OnPlayerDeath()
    {
        cardImage.sprite = deathCard;
        Show();
    }

    private void OnVictory()
    {
        cardImage.sprite = vicotryCard;
        Show();
    }

    private void Show()
    {
        WaveManager.OnGameWon -= OnVictory;
        SceneManager.Instance.Player.Health.OnDeath -= OnPlayerDeath;
        SceneManager.Instance?.Player?.SwitchCameraOff();
        Cursor.lockState = CursorLockMode.None;
        mainElements.SetActive(true);
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }
}
