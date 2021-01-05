using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Assertions;

public class OptionsMenu : MonoBehaviour
{
    public AudioMixer soundsMixer = null;
    public AudioMixer musicMixer = null;


    private void Awake()
    {
        Assert.IsNotNull(soundsMixer);
        Assert.IsNotNull(musicMixer);
    }

    public void SetMusicVolume(float volume)
    {
        musicMixer.SetFloat("musicParameter", Mathf.Log10(volume)*20);
    }

    public void SetSoundsVolume(float volume)
    {
       soundsMixer.SetFloat("soundsParameter", Mathf.Log10(volume)*20);
    }
}
