using UnityEngine;
using UnityEngine.Assertions;


public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    private PlayerController player;
    private int pointsCounter = 0;


    public event System.Action OnPointsChanged;


    public static SceneManager Instance => instance;
    public PlayerController Player => player;
    public int Points => pointsCounter;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        player = FindObjectOfType<PlayerController>();
        Assert.IsNotNull(player);
        player.Health.OnDeath += OnPlayerDeath;
        Enemy.OnAnyEnemyDeath += IncreasePoints;
    }

    private void OnPlayerDeath()
    {
        Enemy.OnAnyEnemyDeath -= IncreasePoints;
    }

    private void IncreasePoints()
    {
        ++pointsCounter;
        OnPointsChanged?.Invoke();
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        Enemy.OnAnyEnemyDeath -= IncreasePoints;
    }
}
