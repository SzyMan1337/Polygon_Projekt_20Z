using UnityEngine;
using UnityEngine.Assertions;
using TMPro;


public class PointCounterGUI : MonoBehaviour
{
    private TextMeshProUGUI text;


    private void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
        Assert.IsNotNull(text);

        Enemy.OnPointsGain += ChangePoints;
    }

    private void ChangePoints()
    {
        text.text = SceneManager.Instance?.Points.ToString();
    }

    private void OnDestroy()
    {
        Enemy.OnPointsGain -= ChangePoints;
    }
}
