using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;
using System.Collections;


public class EndScreen : MonoBehaviour
{
    [SerializeField, Range(0.0f, 5.0f)] private float timeBeforeWinScreen = 1.0f;
    [SerializeField] private GameObject mainElements = null;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite vicotryCard;
    [SerializeField] private Sprite deathCard;
    [SerializeField] private TextMeshProUGUI scoreText;


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
        Assert.IsNotNull(scoreText);
        WaveManager.OnGameWon += OnVictory;
    }

    private void Start()
    {        
        PlayerController player = SceneManager.Instance?.Player; 
        player.Health.OnDeath += OnPlayerDeath;
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
        StartCoroutine(DelayedWin());
        Show();
    }

    private IEnumerator DelayedWin()
    {
        yield return new WaitForSeconds(timeBeforeWinScreen);
        Show();
    }

    private void Show()
    {
        SceneManager.Instance?.Player?.SwitchCameraOff();
        scoreText.text = SceneManager.Instance?.Points.ToString();
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
