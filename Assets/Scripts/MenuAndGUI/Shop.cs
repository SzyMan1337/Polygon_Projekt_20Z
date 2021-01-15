using UnityEngine;

public class Shop : MonoBehaviour
{
    public void AddHealth()
    {
        //UnityEngine.SceneManagement.SceneManager.sceneLoaded += HealthAdd; - alternatywna opcja (nwm czy dziala aktualna)
        var health = FindObjectOfType<PlayerController>().GetComponent<HealthComponent>();
        health.ApplyHeal(10);
    }

    public void AddMovementSpeed()
    {
        var controller = FindObjectOfType<PlayerController>();
        controller.Speed += 2;
    }
}

