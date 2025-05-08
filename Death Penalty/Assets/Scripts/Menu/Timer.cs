using UnityEngine;
using TMPro; // Для TextMeshPro
using System;

public class Timer : MonoBehaviour
{
    [Header("Настройки таймера")]
    [SerializeField] private float startTime = 60f; // Время в секундах
    [SerializeField] private bool countDown = false; // true = обратный отсчет, false = прямой

    [Header("Текстовый вывод (опционально)")]
    [SerializeField] private TextMeshProUGUI timerTextUI; // Для TextMeshPro

    [NonSerialized] static public float currentTime;
    private bool isTimerRunning = false;

    void Start()
    {
        StartTimer();
    }

    void Update()
    {
        if (isTimerRunning && Time.timeScale == 1f)
        {
            UpdateTimer();
        }
    }

    // Запуск таймера
    public void StartTimer()
    {
        currentTime = countDown ? startTime : 0f;
        isTimerRunning = true;
    }

    // Обновление таймера каждый кадр
    private void UpdateTimer()
    {
        if (countDown)
        {
            currentTime -= Time.deltaTime;
            if (currentTime <= 0f)
            {
                currentTime = 0f;
                StopTimer();
                OnTimerEnd();
            }
        }
        else
        {
            currentTime += Time.deltaTime;
        }

        UpdateUIText();
    }

    // Остановка таймера
    public void StopTimer()
    {
        isTimerRunning = false;
    }

    // Действия при завершении времени
    private void OnTimerEnd()
    {
        Debug.Log("Таймер закончился!");
        // Здесь можно добавить:
        // - Завершение уровня
        // - Активацию события
        // - Показ экрана "Время вышло"
    }

    // Обновление текста UI (если подключен)
    private void UpdateUIText()
    {
        if (timerTextUI != null)
        {
            int minutes = Mathf.FloorToInt(currentTime / 60f);
            int seconds = Mathf.FloorToInt(currentTime % 60f);
            timerTextUI.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
    }
}