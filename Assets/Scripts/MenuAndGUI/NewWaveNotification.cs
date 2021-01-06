using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using System.Collections;


public class NewWaveNotification : MonoBehaviour
{
    private TextMeshProUGUI text;
    private const float SHOW_TIME = 2.0f;
    private AudioSource audioSource;


    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(text);

        WaveManager.OnNewWave += ShowNotification;

        text.enabled = false;

        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
    }

    private void ShowNotification()
    {
        audioSource.Play();
        StartCoroutine(DisplayAndWait());
    }

    private IEnumerator DisplayAndWait()
    {
        text.enabled = true;
        yield return new WaitForSeconds(SHOW_TIME);
        text.enabled = false;
    }

    private void OnDestroy()
    {
        WaveManager.OnNewWave -= ShowNotification;
    }
}
