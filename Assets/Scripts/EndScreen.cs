using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;


public class EndScreen : MonoBehaviour
{
    private Canvas canvas;
    private int points = 0;
    private TextMeshProUGUI text;

    
    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        Assert.IsNotNull(canvas);
        //canvas.gameObject.SetActive(false);

        var button = GetComponentInChildren<Button>();
        Assert.IsNotNull(button);
        button.onClick.AddListener(Restart);

        text = canvas.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text);

        canvas.gameObject.SetActive(false);
    }


    private void Start()
    {
        Enemy enemy = FindObjectOfType<Enemy>();
        Assert.IsNotNull(enemy);
        enemy.Health.OnDeath += IncreasePoints;
        PlayerController player = SceneManager.Instance?.Player; 
        player.Health.OnDeath += OnPlayerDeath;

    }


    public void OnPlayerDeath()
    {
        text.text = "Score: " + points;

        Cursor.lockState = CursorLockMode.None;

        canvas.gameObject.SetActive(true);
    }


    private void Restart()
    {
        Cursor.lockState = CursorLockMode.Locked;
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.Instance.name);
    }


    public void IncreasePoints()
    {
        points++;
    }

}
