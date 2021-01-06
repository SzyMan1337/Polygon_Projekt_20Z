using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Assertions;


[CreateAssetMenu(fileName = "Settings", menuName = "ScriptableObjects/Settings", order = 1)]
public class Settings : ScriptableObject
{
	public const float MIN_MOUSE_SENSITIVITY = 1.0f;
	public const float MAX_MOUSE_SENSITIVITY = 1000.0f;
	private const float MIN_DECIBELS = 0.0f;
	private const float MAX_DECIBELS = 10.0f;
	private const float OFF_VALUE = -1.0f;
	private const string MUSIC_PARAMETER = "musicParameter";
	private const string SOUNDS_PARAMETER = "soundsParameter";
	[SerializeField, Range(MIN_MOUSE_SENSITIVITY, MAX_MOUSE_SENSITIVITY)] private float mouseSensitivity = 90.0f;
	[SerializeField] private AudioMixer musicMixer;
	[SerializeField] private AudioMixer soundsMixer;


	public float MouseSensitivity
	{
		get => mouseSensitivity;
		set => mouseSensitivity = Mathf.Clamp(value, MIN_MOUSE_SENSITIVITY, MAX_MOUSE_SENSITIVITY);
	}

	public float MusicVolume
	{
		get => GetVolume(musicMixer, MUSIC_PARAMETER);
		set => SetVolume(value, musicMixer, MUSIC_PARAMETER);
	}

	public float SoundsVolume
	{
		get => GetVolume(soundsMixer, SOUNDS_PARAMETER);
		set => SetVolume(value, soundsMixer, SOUNDS_PARAMETER);
	}


	private void Awake()
	{
		Assert.IsNotNull(musicMixer);
		Assert.IsNotNull(soundsMixer);
	}

	private float ConvertRatioToDecibels(float ratio)
	{
		return Mathf.Log10(ratio * (MAX_DECIBELS - MIN_DECIBELS)) * 20;
	}

	private float ConvertDecibelsToRatio(float decibels)
	{
		return Mathf.Pow(10.0f, decibels / 20) / (MAX_DECIBELS - MIN_DECIBELS);
	}

	private float GetVolume(AudioMixer mixer, string parameterName)
	{
		mixer.GetFloat(parameterName, out var result);
		if (result == OFF_VALUE)
		{
			return 0.0f;
		}
		else
		{
			return ConvertDecibelsToRatio(result);
		}
	}

	private void SetVolume(float volume, AudioMixer mixer, string parameterName)
	{
		if (volume == 0.0f)
		{
			mixer.SetFloat(parameterName, OFF_VALUE);
		}
		else
		{
			mixer.SetFloat(parameterName, ConvertRatioToDecibels(volume));
		}
	}
}
