using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainElements = null;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite vicotryCard;
    [SerializeField] private Sprite deathCard;
    [SerializeField] private AudioSource vicotrySound;
    [SerializeField] private AudioSource defeatSound;
    private AudioSource soundToPlay = null;

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
        Assert.IsNotNull(vicotrySound);
        Assert.IsNotNull(defeatSound);
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
        soundToPlay = defeatSound;
        Show();
    }

    private void OnVictory()
    {
        cardImage.sprite = vicotryCard;
        soundToPlay = vicotrySound;
        Show();
    }

    private void Show()
    {
        StopAllAudio();
        WaveManager.OnGameWon -= OnVictory;
        SceneManager.Instance.Player.Health.OnDeath -= OnPlayerDeath;
        SceneManager.Instance?.Player?.SwitchCamera();
        Cursor.lockState = CursorLockMode.None;
        mainElements.SetActive(true);
        soundToPlay.Play();
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public void StopAllAudio()
    {
        AudioSource[] allAudioSources;
        allAudioSources = FindObjectsOfType(typeof(AudioSource)) as AudioSource[];
        foreach (AudioSource audioS in allAudioSources)
        {
            audioS.Stop();
        }
    }
}