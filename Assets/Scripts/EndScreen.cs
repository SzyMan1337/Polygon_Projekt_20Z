using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;


public class EndScreen : MonoBehaviour
{
    private Canvas canvas;
    private int points = 0;
    private TextMeshProUGUI text;
    private new Camera camera;
    private Button button;


    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();

        canvas = GetComponentInChildren<Canvas>();
        Assert.IsNotNull(canvas);

        button = canvas.GetComponentInChildren<Button>();
        Assert.IsNotNull(button);

        text = canvas.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text);

        Enemy.OnDeath += IncreasePoints;

        camera.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        button.onClick.RemoveAllListeners();

        button.onClick.AddListener(Restart);
        PlayerController player = SceneManager.Instance?.Player; 
        player.Health.OnDeath += OnPlayerDeath;
    }

    public void OnPlayerDeath()
    {
        text.text = "Score: " + points;
        camera.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }

    public void IncreasePoints()
    {
        points++;
    }

}
