using UnityEngine;
using TMPro; // Для TextMeshPro
using System;

public class Timer : MonoBehaviour
{
    [Header("Настройки таймера")]
    [SerializeField] private float startTime = 0f; // Время в секундах
    [SerializeField] private TextMeshProUGUI text;
    [NonSerialized] static public float currentTime;
    private bool isTimerRunning = false;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        text.text = "Ваше хп: " + DamageDetector.playerHP.ToString();
        if (isTimerRunning && Time.timeScale == 1f)
        {
            UpdateTimer();
        }
    }

    // Запуск таймера
    public void StartTimer()
    {
        isTimerRunning = true;
    }

    // Обновление таймера каждый кадр
    private void UpdateTimer()
    {
            currentTime += Time.deltaTime;

    }

    // Остановка таймера
    public void StopTimer()
    {
        isTimerRunning = false;
    }
}