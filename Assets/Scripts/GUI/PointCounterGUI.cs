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
    }

    private void Start()
    {
        SceneManager.Instance.OnPointsChanged += UpdatePoints;
        UpdatePoints();
    }

    private void UpdatePoints()
    {
        text.text = SceneManager.Instance?.Points.ToString();
    }

    private void OnDestroy()
    {
        SceneManager.Instance.OnPointsChanged -= UpdatePoints;
    }
}
