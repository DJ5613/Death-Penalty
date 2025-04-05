/*using UnityEngine;
using UnityEngine.AI;

public class strelok : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    [SerializeField] public float wolkSpeed;
    [SerializeField] public float agroDistance;
    [SerializeField] public float attackDistance;
    Transform target;
    NavMeshPath _cachedPath;

    BaseState currentState;
    public IdleState idleState = new IdleState();
    public AgroState agroState = new AgroState();
    public AttackState attackState = new AttackState();

    public void SwichState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }
    w
    private void Start()
    {
        _cachedPath = new NavMeshPath();
        SwichState(idleState);
    }
    private void Update()
    {
        SetDestination(player);
        navMeshAgent.destination = target.position;
        currentState.UpdateState(this);
        if (DistanceToTarget() < agroDistance) RotateTowardsTarget();
        if (DistanceToTarget() < agroDistance) ShootProjectileAtTarget();
    }

    public void SetSpeed(float newSpeed)
    {
        navMeshAgent.speed = newSpeed;
    }

    public void SetDestination(Transform NewDestination)
    {
        target = NewDestination;
    }

    public float DistanceToTarget()
    {
        if (target == null)
            return Mathf.Infinity;

        if (NavMesh.CalculatePath(transform.position, target.position, NavMesh.AllAreas, _cachedPath))
        {
            float distance = 0f;
            for (int i = 1; i < _cachedPath.corners.Length; i++)
            {
                distance += Vector3.Distance(_cachedPath.corners[i - 1], _cachedPath.corners[i]);
            }
            return distance;
        }

        return Vector3.Distance(transform.position, target.position);
    }

    public void RotateTowardsTarget()
    {
        if (target == null) return;

        Vector3 direction = (target.position - transform.position).normalized;
        direction.y = 0; // Игнорируем наклон по оси Y (если не нужно)

        if (direction != Vector3.zero)
        {
            // Плавный поворот (Quaternion.Lerp или Slerp)
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 5f // Скорость поворота (можно настроить)
            );
        }
    }


    [Header("Projectile Settings")]
    [SerializeField] private GameObject projectilePrefab; // Префаб стрелы/снаряда
    [SerializeField] private Transform shootPoint; // Точка, откуда будет вылетать снаряд
    [SerializeField] private float projectileSpeed = 10f; // Скорость снаряда
    [SerializeField] private float shootCooldown = 2f; // Задержка между выстрелами
    private float lastShootTime; // Время последнего выстрела

    public void ShootProjectileAtTarget()
    {
        // Проверяем, можно ли стрелять (прошло ли время перезарядки)
        if (Time.time - lastShootTime < shootCooldown) return;

        // Проверяем наличие цели и префаба
        if (target == null || projectilePrefab == null || shootPoint == null)
        {
            Debug.LogWarning("Shooting parameters not set properly!");
            return;
        }

        // Создаем снаряд
        GameObject projectile = Instantiate(
            projectilePrefab,
            shootPoint.position,
            Quaternion.identity
        );

        // Рассчитываем направление к цели
        Vector3 direction = (target.position - shootPoint.position).normalized;

        // Настраиваем снаряд
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = direction * projectileSpeed;
        }
        else
        {
            Debug.LogWarning("Projectile prefab has no Rigidbody component!");
        }

        // Настраиваем вращение снаряда, чтобы он смотрел в направлении движения
        projectile.transform.rotation = Quaternion.LookRotation(direction);

        // Запоминаем время последнего выстрела
        lastShootTime = Time.time;
    }
}*/
