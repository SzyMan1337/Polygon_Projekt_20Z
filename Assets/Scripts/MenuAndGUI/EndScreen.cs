using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainElements = null;

    [SerializeField] private Image cardImage;
    [SerializeField] private Sprite deathCard;
    [SerializeField] private Sprite[] victoryCards;

    [SerializeField] private Button returnToMainMenuButton;
    [SerializeField] private Button playButton;
    [SerializeField] private Sprite tryAgainSprite;
    [SerializeField] private Sprite continueSprite;

    [SerializeField] private AudioSource vicotrySound;
    [SerializeField] private AudioSource defeatSound;

    private int numberOfLevels;
    private int levelIndex;
    private AudioSource soundToPlay = null;


    private void Awake()
    {
        numberOfLevels = UnityEngine.SceneManagement.SceneManager.sceneCountInBuildSettings - 1;
        levelIndex = UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex;

        Assert.IsNotNull(mainElements);
        mainElements.gameObject.SetActive(false);

        Assert.IsNotNull(cardImage);
        Assert.IsNotNull(deathCard);
        Assert.IsTrue(numberOfLevels == victoryCards.Length);
        foreach(var victoryCard in victoryCards)
        {
            Assert.IsNotNull(victoryCard);
        }

        Assert.IsNotNull(returnToMainMenuButton);
        returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        Assert.IsNotNull(playButton);
        Assert.IsNotNull(tryAgainSprite);
        Assert.IsNotNull(continueSprite);

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
        playButton.image.sprite = tryAgainSprite;
        playButton.onClick.AddListener(Restart);
        cardImage.sprite = deathCard;
        soundToPlay = defeatSound;
        Show();
    }

    private void OnVictory()
    {
        if (levelIndex == numberOfLevels)
        {
            playButton.image.sprite = tryAgainSprite;
            playButton.onClick.AddListener(Restart);
        }
        else
        {
            playButton.image.sprite = continueSprite;
            playButton.onClick.AddListener(NextLevel);
        }

        cardImage.sprite = victoryCards[levelIndex - 1];
        soundToPlay = vicotrySound;
        Show();
    }

    private void Show()
    {
        StopAllAudio();
        WaveManager.OnGameWon -= OnVictory;
        SceneManager.Instance.Player.Health.OnDeath -= OnPlayerDeath;
        SceneManager.Instance?.Player?.ToggleCamera();
        Cursor.lockState = CursorLockMode.None;
        mainElements.SetActive(true);
        soundToPlay.Play();
    }

    private void Restart()
    {
        SceneManager.pointsCounter = 0;
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }

    private void ReturnToMainMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    private void NextLevel()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(levelIndex + 1);
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