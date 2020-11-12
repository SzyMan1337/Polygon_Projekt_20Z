using UnityEngine;
using UnityEngine.Assertions;


public class SceneManager : MonoBehaviour
{
    private static SceneManager instance;
    private GameObject player;
    

    public static SceneManager Instance => instance;
    public GameObject Player => player;


    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

        player = FindObjectOfType<PlayerController>()?.gameObject;
        Assert.IsNotNull(player);
    }
}
