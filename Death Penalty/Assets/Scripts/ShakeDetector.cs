using UnityEngine;

public class VRShakeDetector : MonoBehaviour
{
    [Header("Shake Settings")]
    [SerializeField] private float shakeSpeedThreshold = 1.5f; // Более чувствительный порог скорости (м/с)
    [SerializeField] private float shakeAccelThreshold = 3.0f; // Порог ускорения для обнаружения резких движений
    [SerializeField] private float checkInterval = 0.1f; // Интервал проверки (секунды)
    [SerializeField] private int positionHistorySize = 10; // Размер истории позиций

    [Header("References")]
    [SerializeField] private AudioSource audioSource; // Ссылка на AudioSource
    [SerializeField] private Rigidbody rb; // Ссылка на Rigidbody

    private Vector3[] positionHistory;
    private Vector3[] velocityHistory;
    private int historyIndex;
    private float lastCheckTime;
    private bool isGrabbed;
    private Vector3 lastVelocity;

    private void Awake()
    {
        if (rb == null) rb = GetComponent<Rigidbody>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();

        positionHistory = new Vector3[positionHistorySize];
        velocityHistory = new Vector3[positionHistorySize];
        historyIndex = 0;
    }

    public void GrabStart()
    {
        rb.isKinematic = false;
        isGrabbed = true;

    }
    public void GrabEnd() => isGrabbed = false;

    private void FixedUpdate()
    {
        if (!isGrabbed || rb == null) return;

        // Записываем текущую позицию и скорость в историю
        positionHistory[historyIndex] = rb.position;
        velocityHistory[historyIndex] = rb.linearVelocity;
        historyIndex = (historyIndex + 1) % positionHistorySize;

        // Проверяем тряску с заданным интервалом
        if (Time.time - lastCheckTime >= checkInterval)
        {
            CheckForShake();
            lastCheckTime = Time.time;
        }

        lastVelocity = rb.linearVelocity;
    }

    private void CheckForShake()
    {
        // 1. Проверка мгновенной скорости
        if (rb.linearVelocity.magnitude > shakeSpeedThreshold)
        {
            TriggerShake();
            return;
        }

        // 2. Проверка резкого изменения скорости (ускорения)
        Vector3 acceleration = (rb.linearVelocity - lastVelocity) / Time.fixedDeltaTime;
        if (acceleration.magnitude > shakeAccelThreshold)
        {
            TriggerShake();
            return;
        }

        // 3. Проверка сложного движения (изменение направления)
        float directionChange = CalculateDirectionVariance();
        if (directionChange > 0.8f && rb.linearVelocity.magnitude > shakeSpeedThreshold * 0.7f)
        {
            TriggerShake();
        }
    }

    private float CalculateDirectionVariance()
    {
        Vector3 avgDirection = Vector3.zero;
        int validSamples = 0;

        // Вычисляем среднее направление
        for (int i = 0; i < velocityHistory.Length; i++)
        {
            if (velocityHistory[i] != Vector3.zero)
            {
                avgDirection += velocityHistory[i].normalized;
                validSamples++;
            }
        }

        if (validSamples == 0) return 0f;

        avgDirection /= validSamples;
        float variance = 0f;

        // Вычисляем дисперсию направлений
        for (int i = 0; i < velocityHistory.Length; i++)
        {
            if (velocityHistory[i] != Vector3.zero)
            {
                variance += Vector3.Distance(velocityHistory[i].normalized, avgDirection);
            }
        }

        return variance / validSamples;
    }

    private void TriggerShake()
    {
        Debug.Log("Shake detected! Velocity: " + rb.linearVelocity.magnitude + " m/s");

        // Воспроизведение звука
        if (audioSource != null && !audioSource.isPlaying)
        {
            audioSource.Play();
        }

        // Визуальная обратная связь
        Renderer rend = GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material.color = new Color(
                Random.Range(0.7f, 1f),
                Random.Range(0.7f, 1f),
                Random.Range(0.7f, 1f)
            );
        }
    }
}