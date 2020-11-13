using UnityEngine;
using UnityEngine.Assertions;


public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    private PlayerController player;
    

    public static SceneManager Instance => instance;
    public PlayerController Player => player;


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
}
