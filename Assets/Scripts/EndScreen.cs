using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using TMPro;


public class EndScreen : MonoBehaviour
{
    [SerializeField] private GameObject mainElements = null;
    
    private TextMeshProUGUI text;
    private new Camera camera;
    private Button button;


    private void Awake()
    {
        Assert.IsNotNull(mainElements);
        mainElements.gameObject.SetActive(false);

        camera = GetComponentInChildren<Camera>(true);
        Assert.IsNotNull(camera);

        button = GetComponentInChildren<Button>(true);
        Assert.IsNotNull(button);
        button.onClick.AddListener(Restart);

        text = GetComponentInChildren<TextMeshProUGUI>(true);
        Assert.IsNotNull(text);
    }

    private void Start()
    {        
        PlayerController player = SceneManager.Instance?.Player; 
        player.Health.OnDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        int points = SceneManager.Instance.Points;
        text.text = "Score: " + points;
        Cursor.lockState = CursorLockMode.None;
        mainElements.SetActive(true);
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}
