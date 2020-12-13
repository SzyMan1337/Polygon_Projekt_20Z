using UnityEngine;
using UnityEngine.Assertions;
using TMPro;
using System.Collections;


public class NewWaveNotification : MonoBehaviour
{
    private TextMeshProUGUI text;
    private const float TIME = 2.0f;

    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(text);

        WaveManager.OnNewWave += ShowNotification;

        text.enabled = false;
    }

    private void ShowNotification()
    {
        StartCoroutine(DisplayAndWait());
    }
    private IEnumerator DisplayAndWait()
    {
        text.enabled = true;
        yield return new WaitForSeconds(TIME);
        text.enabled = false;
    }

    private void OnDestroy()
    {
        WaveManager.OnNewWave -= ShowNotification;
    }
}
