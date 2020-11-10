using UnityEngine;
using UnityEngine.Assertions;


public class SceneManager : MonoBehaviour
{
    public GameObject player;

    private static SceneManager instance;


    public static SceneManager Instance => instance;
   

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
