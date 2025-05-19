using UnityEngine;
using TMPro; // Для TextMeshPro
using System;

public class Timer : MonoBehaviour
{
    [Header("Настройки таймера")]
    [SerializeField] private float startTime = 0f; // Время в секундах
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI timer;
    [NonSerialized] static public float currentTime;
    private bool isTimerRunning = false;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        text.text = "HP: " + DamageDetector.playerHP.ToString();
        int minutes = Mathf.FloorToInt(Timer.currentTime / 60f);
        int seconds = Mathf.FloorToInt(Timer.currentTime % 60f);
        timer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
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