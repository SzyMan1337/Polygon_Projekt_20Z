using UnityEngine;
using UnityEngine.Assertions;


public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    private PlayerController player;
    public static int pointsOfPlayer = 0;

    public static SceneManager Instance => instance;
    public PlayerController Player => player;
    public int Points => pointsOfPlayer;


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
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
        pointsOfPlayer = 0;
    }
}
