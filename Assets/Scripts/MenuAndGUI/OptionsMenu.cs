using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;


public class OptionsMenu : MonoBehaviour
{
	[SerializeField] private Settings settings = null;
	[SerializeField] private Slider mouseSensitivitySlider = null;
	[SerializeField] private Slider musicVolumeSlider = null;
	[SerializeField] private Slider soundsVolumeSlider = null;

	private void Awake()
	{
		Assert.IsNotNull(settings);
		
		Assert.IsNotNull(mouseSensitivitySlider);
		mouseSensitivitySlider.onValueChanged.AddListener(UpdateMouseSensitivity);
		mouseSensitivitySlider.minValue = Settings.MIN_MOUSE_SENSITIVITY;
		mouseSensitivitySlider.maxValue = Settings.MAX_MOUSE_SENSITIVITY;
		mouseSensitivitySlider.value = settings.MouseSensitivity;

		Assert.IsNotNull(musicVolumeSlider);
		musicVolumeSlider.onValueChanged.AddListener(UpdateMusicVolume);
		musicVolumeSlider.minValue = 0.0f;
		musicVolumeSlider.maxValue = 1.0f;
		musicVolumeSlider.value = settings.MusicVolume;

		Assert.IsNotNull(soundsVolumeSlider);
		soundsVolumeSlider.onValueChanged.AddListener(UpdateSoundsVolume);
		soundsVolumeSlider.minValue = 0.0f;
		soundsVolumeSlider.maxValue = 1.0f;
		soundsVolumeSlider.value = settings.SoundsVolume;
	}

	private void UpdateMouseSensitivity(float value)
	{
		settings.MouseSensitivity = value;
	}

	private void UpdateMusicVolume(float value)
	{
		settings.MusicVolume = value;
	}

	private void UpdateSoundsVolume(float value)
	{
		settings.SoundsVolume = value;
	}
}
