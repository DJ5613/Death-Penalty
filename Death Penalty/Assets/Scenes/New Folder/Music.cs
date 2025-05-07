using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Music : MonoBehaviour
{
    [Header("Audio Mixer References")]
    [SerializeField] private AudioMixer audioMixer;

    [Header("Volume Sliders")]
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider effectsVolumeSlider;

    [Header("Volume Parameters")]
    [SerializeField] private string musicVolumeParam = "Music";
    [SerializeField] private string effectsVolumeParam = "Effects";

    private void Awake()
    {
        // Подписываемся на события слайдеров
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
        effectsVolumeSlider.onValueChanged.AddListener(SetEffectsVolume);
    }

    private void Start()
    {
        // Инициализация значений слайдеров из сохраненных настроек
        musicVolumeSlider.value = PlayerPrefs.GetFloat(musicVolumeParam, 0.75f);
        effectsVolumeSlider.value = PlayerPrefs.GetFloat(effectsVolumeParam, 0.75f);
    }

    private void SetMusicVolume(float value)
    {
        SetVolume(musicVolumeParam, value);
    }

    private void SetEffectsVolume(float value)
    {
        SetVolume(effectsVolumeParam, value);
    }

    private void SetVolume(string parameter, float value)
    {
        // Преобразуем линейное значение (0-1) в логарифмическое (dB)
        //float volume = Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20;
        float volume = value * 100 - 80;
        audioMixer.SetFloat(parameter, volume);

        // Сохраняем настройки
        PlayerPrefs.SetFloat(parameter, value);
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
        effectsVolumeSlider.onValueChanged.RemoveListener(SetEffectsVolume);

    }
}