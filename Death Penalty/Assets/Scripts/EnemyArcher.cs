using UnityEngine;

public class ArcherEnemy : MonoBehaviour
{
    [Header("Стрельба")]
    public GameObject arrowPrefab; // Префаб стрелы
    public Transform shootPoint; // Точка, откуда выпускаются стрелы
    public float shootInterval = 2f; // Интервал между выстрелами
    public float arrowSpeed = 10f; // Начальная скорость стрелы
    public float arrowGravity = 9.8f; // Гравитация для стрелы

    [Header("Настройки цели")]
    public Transform target; // Цель (обычно игрок)
    public float maxShootDistance = 15f; // Максимальная дистанция стрельбы

    private float shootTimer;

    void Start()
    {
        // Если цель не назначена, попытаться найти игрока
        if (target == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) target = player.transform;
        }

        shootTimer = shootInterval; // Начинаем с возможности выстрела
    }

    void Update()
    {
        if (target == null) return;

        // Проверяем расстояние до цели
        float distanceToTarget = Vector3.Distance(transform.position, target.position);
        if (distanceToTarget > maxShootDistance) return;

        // Таймер для стрельбы
        shootTimer -= Time.deltaTime;
        if (shootTimer <= 0f)
        {
            Shoot();
            shootTimer = shootInterval;
        }
    }

    void Shoot()
    {
        if (arrowPrefab == null || shootPoint == null) return;

        // Создаем стрелу
        GameObject arrow = Instantiate(arrowPrefab, shootPoint.position, shootPoint.rotation);

        // Настраиваем Rigidbody для баллистики
        Rigidbody rb = arrow.GetComponent<Rigidbody>();
        if (rb == null)
        {
            rb = arrow.AddComponent<Rigidbody>();
        }

        // Рассчитываем направление к цели с учетом гравитации (баллистическая траектория)
        Vector3 direction = target.position - shootPoint.position;//CalculateBallisticVelocity(shootPoint.position, target.position, arrowGravity);

        // Применяем силу к стреле
        rb.velocity = direction * arrowSpeed;

        // Включаем коллизию (если она была выключена в префабе)
        Collider arrowCollider = arrow.GetComponent<Collider>();
        if (arrowCollider != null)
        {
            arrowCollider.enabled = true;
        }
    }

    // Расчет баллистической траектории
    /*Vector3 CalculateBallisticVelocity(Vector3 origin, Vector3 target, float gravity)
    {
        Vector3 direction = target - origin;
        float height = direction.y;
        direction.y = 0;
        float distance = direction.magnitude;
        direction.y = distance; // Угол 45 градусов для максимальной дальности
        distance += height;

        if (distance < 0.1f)
            return Vector3.zero;

        float velocity = Mathf.Sqrt(distance * gravity);
        return velocity * direction.normalized;
    }*/

    // Визуализация радиуса атаки в редакторе
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, maxShootDistance);
    }
}