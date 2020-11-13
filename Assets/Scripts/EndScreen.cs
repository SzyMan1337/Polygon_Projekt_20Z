using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;

public class EndScreen : MonoBehaviour
{
    private Canvas canvas;
    private int points = 0;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        Assert.IsNotNull(canvas);
        canvas.gameObject.SetActive(false);

        var button = GetComponentInChildren<Button>();
        Assert.IsNotNull(button);
        button.onClick.AddListener(Restart);

        var text = canvas.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text);
        text.text = "Score: " + points;
    }

    private void Start()
    {
        var player = SceneManager.Instance?.Player;
        Assert.IsNotNull(player);
        player.Health.OnDeath += OnPlayerDeath;
                
    }

    private void Update()
    {
        Enemy[] enemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in enemies)
        {
            enemy.Health.OnDeath += CountPoints;
        }
    }


    private void OnPlayerDeath()
    {
        canvas.gameObject.SetActive(true);
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
        canvas.gameObject.SetActive(false);
        points = 0;
    }

    private void CountPoints()
    {
        points++;
    }

}
