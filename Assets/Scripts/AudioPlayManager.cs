//using UnityEngine;
//using UnityEngine.Assertions;

//public class AudioPlayManager : MonoBehaviour
//{
//    private static AudioPlayManager instance = null;
//    private AudioSource audio;

//    private void Awake()
//    {
//        if (instance == null)
//        {
//            instance = this;
//            DontDestroyOnLoad(gameObject);
//            return;
//        }
//        if (instance == this) return;
//        Destroy(gameObject);
//    }

//    void Start()
//    {
//        audio = GetComponent<AudioSource>();
//        audio.Play();
//        Debug.Log(audio.clip);
//    }
//}

using UnityEngine;
using UnityEngine.Assertions;

public class AudioPlayManager : MonoBehaviour
{
    public AudioClip[] clips;
    private AudioSource audioSource;
    
    private void Start()
    {
        audioSource = FindObjectOfType<AudioSource>();
        audioSource.loop = false;
        audioSource.clip = clips[0];
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.D)|| Input.GetKeyDown(KeyCode.W))
        {
            audioSource.Play();
        }
    }



}