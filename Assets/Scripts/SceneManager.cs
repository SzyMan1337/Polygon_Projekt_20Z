using UnityEngine;
using UnityEngine.Assertions;


public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    private PlayerController player;
    private int points = 0;

    public static event System.Action OnPointsGain;

    public static SceneManager Instance => instance;
    public PlayerController Player => player;
    public int Points => points;


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

        Enemy.OnAnyEnemyDeath += IncreasePoints;
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        Enemy.OnAnyEnemyDeath -= IncreasePoints;
    }

    private void IncreasePoints()
    {
        ++points;
        OnPointsGain?.Invoke();
        Debug.Log(points + gameObject.name);
    }
}
