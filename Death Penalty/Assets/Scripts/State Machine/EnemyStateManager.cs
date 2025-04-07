using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    [SerializeField] public Animator animator;
    public float wolkSpeed;
    public float agroDistance;
    public float simpleAttackDistance;
    public float comboAttackDistance;
    public float comboAttackSpeed;
    public float enemyHP;
    public float enemyDamage;
    Transform target;
    NavMeshPath _cachedPath;

    BaseState currentState;
    public IdleState idleState = new IdleState();
    public AgroState agroState = new AgroState();
    public SimpleAttackState simpleAttackState = new SimpleAttackState();
    public ComboAttackState comboAttackState = new ComboAttackState();
    public DeathState deathState = new DeathState();

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
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        _cachedPath = new NavMeshPath();
        SwichState(idleState);
    }
    private void Update()
    {
        SetDestination(player);
        navMeshAgent.destination = target.position;
        currentState.UpdateState(this);
        if (DistanceToTarget() < agroDistance && !animator.GetBool("IsDeath")) RotateTowardsTarget();
        if (enemyHP <= 0) animator.SetBool("IsDeath", true);
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

    void CheckState()
    {
        if (DistanceToTarget() >= comboAttackDistance)
        {
            SwichState(agroState);
            return;
        }
        if (DistanceToTarget() > simpleAttackDistance && DistanceToTarget() < comboAttackDistance)
        {
            SwichState(comboAttackState);
            return;
        }
        if (DistanceToTarget() <= simpleAttackDistance)
        {
            SwichState(simpleAttackState);
            return;
        }
    }

    void ResetSpeed() { SetSpeed(0); }
    void SetComboSpeed() { SetSpeed(comboAttackSpeed); }
    
}
