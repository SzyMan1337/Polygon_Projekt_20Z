﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;
using System.Collections;
using TMPro;


public class EndScreen : MonoBehaviour
{
    [SerializeField] private Canvas canvas = null;
    [SerializeField] private Camera camera = null;
    [SerializeField] private Button tryAgainButton = null;
    [SerializeField] private TextMeshProUGUI text = null;


    private void Awake()
    {
        Assert.IsNotNull(canvas);
        Assert.IsNotNull(camera);
        Assert.IsNotNull(tryAgainButton);
        Assert.IsNotNull(text);

        camera.gameObject.SetActive(false);
        canvas.gameObject.SetActive(false);
    }

    private void Start()
    {
        tryAgainButton.onClick.RemoveAllListeners();
        tryAgainButton.onClick.AddListener(Restart);

        PlayerController player = SceneManager.Instance?.Player; 
        player.Health.OnDeath += OnPlayerDeath;
    }

    private void OnPlayerDeath()
    {
        text.text = SceneManager.Instance?.Points.ToString();
        camera.gameObject.SetActive(true);
        canvas.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    private void Restart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
}