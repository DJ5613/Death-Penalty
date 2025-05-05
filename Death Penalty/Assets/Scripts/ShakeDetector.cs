using UnityEngine;

public class ShakeDetector : MonoBehaviour
{
    [Header("Настройки тряски")]
    [SerializeField] private float shakeSpeedThreshold = 2f; // Минимальная скорость для активации (м/с)
    [SerializeField] private float shakeDistanceThreshold = 0.3f; // Минимальное смещение за интервал
    [SerializeField] private float checkInterval = 0.1f; // Время между проверками (секунды)
    [SerializeField] private int bufferSize = 5; // Размер буфера для сглаживания
    private AudioSource audioSource;

    private Rigidbody rb;
    public bool isGrabbed;
    private Vector3[] positionBuffer;
    private int bufferIndex;
    private float lastCheckTime;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        positionBuffer = new Vector3[bufferSize];
    }

    public void GrabStart() => isGrabbed = true;
    public void GrabEnd() => isGrabbed = false;

    private void Update()
    {
        if (!isGrabbed) return;

        if (Time.time - lastCheckTime >= checkInterval)
        {
            UpdatePositionBuffer();
            CheckForShake();
            lastCheckTime = Time.time;
        }
    }

    private void UpdatePositionBuffer()
    {
        positionBuffer[bufferIndex] = rb.position;
        bufferIndex = (bufferIndex + 1) % bufferSize;
    }

    private void CheckForShake()
    {
        // 1. Проверка по мгновенной скорости (быстрая реакция)
        if (rb.linearVelocity.magnitude > shakeSpeedThreshold)
        {
            TriggerShake();
            return;
        }

        //// 2. Проверка по общему смещению (для медленных, но размашистых движений)
        //Vector3 totalMovement = Vector3.zero;
        //int validSamples = 0;

        //for (int i = 0; i < bufferSize - 1; i++)
        //{
        //    int currentIndex = (bufferIndex + i) % bufferSize;
        //    int nextIndex = (bufferIndex + i + 1) % bufferSize;

        //    if (positionBuffer[nextIndex] == Vector3.zero ||
        //        positionBuffer[currentIndex] == Vector3.zero)
        //        continue;

        //    totalMovement += positionBuffer[nextIndex] - positionBuffer[currentIndex];
        //    validSamples++;
        //}

        //if (validSamples > 0 && totalMovement.magnitude / validSamples > shakeDistanceThreshold)
        //{
        //    TriggerShake();
        //}
    }

    private void TriggerShake()
    {
        Debug.Log("Объект тряхнули! Активируем бафф");
        // Ваш код активации баффа здесь
        audioSource.Play();

        // Пример: визуальная обратная связь
        GetComponent<Renderer>().material.color = new Color(
            Random.value,
            Random.value,
            Random.value
        );
    }
}
