using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;

public class EndScreen : MonoBehaviour
{
    private Canvas canvas;
    public int points = 0;

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        Assert.IsNotNull(canvas);
        canvas.gameObject.SetActive(false);

        var button = GetComponentInChildren<Button>();
        Assert.IsNotNull(button);
        button.onClick.AddListener(Restart);
    }

    public void OnPlayerDeath()
    {
        var text = canvas.GetComponentInChildren<TextMeshProUGUI>();
        Assert.IsNotNull(text);
        text.text = "Score: " + points;

        Cursor.lockState = CursorLockMode.None;

        canvas.gameObject.SetActive(true);
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(SceneManager.Instance.name);

        Cursor.lockState = CursorLockMode.Locked;

        canvas.gameObject.SetActive(false);
        
        points = 0;
    }

    public void IncreasPoints()
    {
        points++;
    }

}
