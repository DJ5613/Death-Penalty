using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class BossStateManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    [SerializeField] public Animator animator;
    public float walkSpeed;
    public float agroDistance;
    //public float simpleAttackDistance;
    //public float comboAttackDistance;
    //public float comboAttackSpeed;
    public float enemyHP;
    public float enemyDamage;
    Transform target;
    NavMeshPath _cachedPath;

    BaseState currentState;
    public IdleState idleState = new IdleState();
    public AgroState agroState = new AgroState();
    //public SimpleAttackState simpleAttackState = new SimpleAttackState();
    //public ComboAttackState comboAttackState = new ComboAttackState();
    public DeathState deathState = new DeathState();
    public static bool playerInRoom = true;

    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;
    private bool isRotate;

    [SerializeField] private GameObject resultMenu;
    public void SwichState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    private void Start()
    {
        switch (SwitchDifficulty.dif_num)
        {
            case 1:
                walkSpeed = (float)(walkSpeed * 0.7);
                enemyHP = (float)(enemyHP * 0.5);
                enemyDamage = (float)(enemyDamage * 0.5);
                break;
            case 2:
                break;
            case 3:
                walkSpeed = (float)(walkSpeed * 1.2);
                enemyHP = (float)(enemyHP * 1.5);
                enemyDamage = (float)(enemyDamage * 1.5);
                break;
        }
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        _cachedPath = new NavMeshPath();
        SwichState(idleState);
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

    }
    private void Update()
    {
        if (playerInRoom)
        {
            float speed = navMeshAgent.velocity.magnitude;
            animator.SetFloat("Speed", speed); // "Speed" — параметр в аниматоре
            SetDestination(player);
            navMeshAgent.destination = target.position;
            currentState.UpdateState(this);
            //if (DistanceToTarget() < agroDistance && !animator.GetBool("IsDeath")) RotateTowardsTarget();
            if (isRotate) RotateTowardsTarget();
        }
        if (enemyHP <= 0)
        {
            //animator.SetBool("IsDeath", true);
            ResultsMenu.kill_score += 1;
            Debug.Log("БОСС УМЕР");
            resultMenu.SetActive(true);
        }
        SynchronizeAnimatorAndAgent();
        //if (navMeshAgent.velocity.sqrMagnitude > 0.01f)
        //{
        //    Quaternion targetRotation = Quaternion.LookRotation(navMeshAgent.velocity.normalized);
        //    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        //}
    }

    void OnAnimatorMove()
    {
        // Корректируем позицию вручную, учитывая расчёт NavMeshAgent
        Vector3 newPosition = animator.rootPosition;
        newPosition.y = navMeshAgent.nextPosition.y;  // Сохраняем Y-позицию от агента (чтобы не было "прыжков")
        transform.position = newPosition;
        transform.rotation = animator.rootRotation;
        navMeshAgent.nextPosition = newPosition;

        // Если нужно, можно смещать позицию вперёд (для более агрессивного преследования)
        transform.position += transform.forward * walkSpeed * Time.deltaTime;
    }

    public void SetSpeed(float newSpeed)
    {
        navMeshAgent.speed = newSpeed;
    }

    public void SetDestination(Transform NewDestination)
    {
        target = NewDestination;
    }

    public void Rotation(int value)
    {
        if (value == 0) isRotate = false;
        else isRotate = true;
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

    private void RotateTowardsTarget()
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

    private void SynchronizeAnimatorAndAgent()
    {
        Vector3 worldDeltaPosition = navMeshAgent.nextPosition - transform.position;
        worldDeltaPosition.y = 0;

        float dx = Vector3.Dot(transform.right, worldDeltaPosition);
        float dy = Vector3.Dot(transform.forward, worldDeltaPosition);
        Vector2 deltaPosition = new Vector2(dx, dy);

        float smooth = Mathf.Min(1, Time.deltaTime / 0.1f);
        SmoothDeltaPosition = Vector2.Lerp(SmoothDeltaPosition, deltaPosition, smooth);

        Velocity = SmoothDeltaPosition / Time.deltaTime;
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            Velocity = Vector2.Lerp(
                Vector2.zero,
                Velocity,
                navMeshAgent.remainingDistance / navMeshAgent.stoppingDistance
            );
        }

        bool shouldMove = Velocity.magnitude > 0.5f
            && navMeshAgent.remainingDistance > navMeshAgent.stoppingDistance;

        animator.SetBool("move", shouldMove);
        animator.SetFloat("locomotion", Velocity.magnitude);

        float deltaMagnitude = worldDeltaPosition.magnitude;
        if (deltaMagnitude > navMeshAgent.radius / 2f)
        {
            transform.position = Vector3.Lerp(
                animator.rootPosition,
                navMeshAgent.nextPosition,
                smooth
            );
        }
    }
}
