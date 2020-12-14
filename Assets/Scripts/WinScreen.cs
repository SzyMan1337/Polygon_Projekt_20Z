using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;
using TMPro;


public class WinScreen : MonoBehaviour
{
    [SerializeField, Range(0.0f, 5.0f)] private float timeBeforeWinScreen = 0.5f;
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private Camera camera = null;
    [SerializeField] private Button tryAgainButton = null;
    [SerializeField] private Button backToMainMenuButton = null;
    [SerializeField] private TextMeshProUGUI text = null;
    PlayerController player = null;


    private void Awake()
    {
        Assert.IsNotNull(canvas);
        Assert.IsNotNull(camera);
        Assert.IsNotNull(tryAgainButton);
        Assert.IsNotNull(backToMainMenuButton);
        Assert.IsNotNull(text);

        camera.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        tryAgainButton.onClick.RemoveAllListeners();
        tryAgainButton.onClick.AddListener(Restart);
        backToMainMenuButton.onClick.RemoveAllListeners();
        //backToMainMenuButton.onClick.AddListener();       TO DO: move to main menu

        player = SceneManager.Instance?.Player;

        WaveManager.OnWinningGame += Win;
    }

    private void Win()
    {
        StartCoroutine(DelayedWin());
    }

    private IEnumerator DelayedWin()
    {
        yield return new WaitForSeconds(timeBeforeWinScreen);
        player.SwitchCameraOff();
        text.text = SceneManager.Instance?.Points.ToString();
        camera.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        WaveManager.OnWinningGame -= Win;
    }
}