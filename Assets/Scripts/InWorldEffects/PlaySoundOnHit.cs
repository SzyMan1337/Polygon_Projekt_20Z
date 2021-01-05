using UnityEngine;
using UnityEngine.Assertions;


public class PlaySoundOnHit : MonoBehaviour
{
    private AudioSource audioSource = null;
    private HealthComponent healthComponent = null;


    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        Assert.IsNotNull(audioSource);
        healthComponent = GetComponent<HealthComponent>();
        Assert.IsNotNull(healthComponent);
        healthComponent.OnHit += OnHit;
    }

    private void OnHit()
    {
        audioSource.Play();
    }
}
