using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{

    [SerializeField] private Slider volumeSlider;    // Ссылка на UI Slider
    [SerializeField] private string volumeParameter = "MasterVolume"; // Параметр микшера

    private void Start()
    {
        // Загружаем сохранённое значение громкости (если есть)
        float savedVolume = PlayerPrefs.GetFloat(volumeParameter, 0.75f);

        // Устанавливаем начальное значение слайдера
        volumeSlider.value = savedVolume;
        SetVolume(savedVolume);

        // Подписываемся на изменение слайдера
        volumeSlider.onValueChanged.AddListener(SetVolume);
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        // Сохраняем значение
        PlayerPrefs.SetFloat(volumeParameter, volume);
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        volumeSlider.onValueChanged.RemoveListener(SetVolume);
    }
}