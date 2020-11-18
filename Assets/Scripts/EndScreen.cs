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
    
    private void Awake()
    {
        camera = GetComponentInChildren<Camera>();

        canvas = GetComponentInChildren<Canvas>();
        Assert.IsNotNull(canvas);

        var button = canvas.GetComponentInChildren<Button>();
        Assert.IsNotNull(button);
        button.onClick.AddListener(Restart);
        
        text = canvas.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text);

        camera.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }


    private void Start()
    {
        PlayerController player = SceneManager.Instance?.Player; 
        player.Health.OnDeath += OnPlayerDeath;
    }


    private void Update()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();

        foreach (var en in enemies)
        {
            en.Health.OnDeath -= IncreasePoints;
            en.Health.OnDeath += IncreasePoints;
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            Restart();
        }

    }


    public void OnPlayerDeath()
    {
        text.text = "Score: " + points;

        Cursor.lockState = CursorLockMode.None;
        camera.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
    }


    private void Restart()
    {
        camera.gameObject.SetActive(false);
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    public void IncreasePoints()
    {
        points++;
    }

}
