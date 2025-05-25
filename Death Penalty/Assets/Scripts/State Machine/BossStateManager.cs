using System;
using UnityEngine;
using UnityEngine.AI;

public class BossStateManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    [SerializeField] public Animator animator;
    [SerializeField] float rotationSpeed;
    [SerializeField] float rotationOffset;
    public float walkSpeed; //по сути бесполезная
    public float simpleAttackDistance;
    public float comboAttackDistance;
    public float comboAttackRange;
    public float enemyHP;
    private float halfHP;
    public float enemyDamage;
    Transform target;
    NavMeshPath _cachedPath;

    BaseState currentState;
    public BossAgroState bossAgroState = new BossAgroState();
    public BossComboState bossComboState = new BossComboState();
    public BossAttackState bossAttackState = new BossAttackState();
    public BossDeathState bossDeathState = new BossDeathState();
    public static bool playerInRoom;
    [NonSerialized] public bool isRotate = true;


    private Vector2 Velocity;
    private Vector2 SmoothDeltaPosition;
    private int attackNum = 0;
    private int maxAttackNum = 4;    
    public int AttackNum
    {
        get { return attackNum; }
        set 
        { 
            if (value > maxAttackNum) attackNum = 1;
            else attackNum = value;
        }
    }

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
                //walkSpeed = (float)(walkSpeed * 0.7);
                enemyHP = (float)(enemyHP * 0.7);
                enemyDamage = (float)(enemyDamage * 0.5);
                break;
            case 2:
                break;
            case 3:
                //walkSpeed = (float)(walkSpeed * 1.2);
                enemyHP = (float)(enemyHP * 1.5);
                enemyDamage = (float)(enemyDamage * 1.5);
                break;
        }
        playerInRoom = false;
        halfHP = enemyHP / 2;
        Debug.Log($"половина хп: {halfHP}");
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        _cachedPath = new NavMeshPath();
        navMeshAgent.updatePosition = false;
        navMeshAgent.updateRotation = false;

    }
    private void Update()
    {
        if (playerInRoom)
        {
            SetDestination(player);
            navMeshAgent.destination = target.position;
            currentState.UpdateState(this);
            if (attackNum == maxAttackNum) animator.SetBool("IsTired", true);
            if (isRotate) RotateTowardsTarget();
        }
        if (enemyHP <= 0)
        {
            ResultsMenu.kill_score += 1;
            Debug.Log("БОСС УМЕР");
            resultMenu.SetActive(true);
        }
        if (enemyHP <= halfHP)
        {
            animator.SetBool("SecondStage", true);
            maxAttackNum = 7;
        }
        SynchronizeAnimatorAndAgent();
    }

    void OnAnimatorMove()
    {
        // Корректируем позицию вручную, учитывая расчёт NavMeshAgent
        Vector3 newPosition = animator.rootPosition;
        newPosition.y = navMeshAgent.nextPosition.y;  // Сохраняем Y-позицию от агента (чтобы не было "прыжков")
        transform.position = newPosition;
        transform.rotation = animator.rootRotation;
        navMeshAgent.nextPosition = newPosition;
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

    //private void RotateTowardsTarget()
    //{
    //    if (target == null) return;

    //    Vector3 direction = (target.position - transform.position).normalized;
    //    direction.y = 0; // Игнорируем наклон по оси Y (если не нужно)

    //    if (direction != Vector3.zero)
    //    {
    //        Quaternion targetRotation = Quaternion.LookRotation(direction);
    //        targetRotation *= Quaternion.Euler(0, rotationOffset, 0);

    //        // Плавный поворот с учётом коррекции
    //        transform.rotation = Quaternion.Slerp(
    //            transform.rotation,
    //            targetRotation,
    //            Time.deltaTime * rotationSpeed
    //            );
    //    }
    //}

    private void RotateTowardsTarget()
    {
        if (navMeshAgent.pathPending)
            return;

        Vector3 direction = navMeshAgent.steeringTarget - transform.position;
        direction.y = 0f;

        if (direction.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction.normalized);
            targetRotation *= Quaternion.Euler(0, rotationOffset, 0);

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * rotationSpeed
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

    void SwichAttack()
    {
        if (animator.GetBool("IsUndercuting"))
        {
            animator.SetBool("IsUndercuting", false);
            animator.SetBool("IsDownAttacking", true);
        }
        else
        {
            animator.SetBool("IsUndercuting", true);
            animator.SetBool("IsDownAttacking", false);
        }
    }
    void Tired()
    {
        AttackNum++;
        if (AttackNum == maxAttackNum)
        {
            animator.SetBool("IsTired", true);
            attackNum = 0;
        }
        Debug.Log("номер атаки:" + AttackNum + "," + maxAttackNum);
    }

    void TiredReset()
    {
        animator.SetBool("IsTired", false);
    }
    void Rotate(int value)
    {
        if (value == 1) isRotate = true;
        else isRotate = false;
    }
    void CheckState()
    {
        //if (DistanceToTarget() >= comboAttackDistance)
        //{
        //    SwichState(agroState);
        //    return;
        //}
        //if (DistanceToTarget() > simpleAttackDistance && DistanceToTarget() < comboAttackDistance)
        //{
        //    SwichState(comboAttackState);
        //    return;
        //}
        //if (DistanceToTarget() <= simpleAttackDistance)
        //{
        //    SwichState(simpleAttackState);
        //    return;
        //}
    }
}

//using UnityEngine;
//using UnityEngine.AI;

//public class BossStateManager : MonoBehaviour
//{
//    [Header("References")]
//    [SerializeField] private NavMeshAgent navMeshAgent;
//    [SerializeField] private Transform player;
//    [SerializeField] private Animator animator;
//    [SerializeField] private GameObject resultMenu;

//    [Header("Settings")]
//    public float walkSpeed = 3.5f;
//    public float agroDistance = 10f;
//    public float enemyHP = 100f;
//    public float enemyDamage = 10f;

//    // States
//    private BaseState currentState;
//    public IdleState idleState = new IdleState();
//    public AgroState agroState = new AgroState();
//    public DeathState deathState = new DeathState();
//    public static bool playerInRoom = true;

//    private NavMeshPath _cachedPath;
//    private Transform _target;
//    private bool _shouldRotate;

//    private void Start()
//    {
//        ApplyDifficultySettings();
//        InitializeComponents();
//        SwichState(idleState);
//    }

//    private void ApplyDifficultySettings()
//    {
//        switch (SwitchDifficulty.dif_num)
//        {
//            case 1:
//                walkSpeed *= 0.7f;
//                enemyHP *= 0.5f;
//                enemyDamage *= 0.5f;
//                break;
//            case 3:
//                walkSpeed *= 1.2f;
//                enemyHP *= 1.5f;
//                enemyDamage *= 1.5f;
//                break;
//        }
//    }

//    public void SwichState(BaseState newState)
//    {
//        if (currentState != null)
//        {
//            currentState.ExitState(this);
//        }
//        currentState = newState;
//        currentState.EnterState(this);
//    }

//    private void InitializeComponents()
//    {
//        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
//        _cachedPath = new NavMeshPath();

//        navMeshAgent.updatePosition = false;
//        navMeshAgent.updateRotation = false;
//        navMeshAgent.speed = walkSpeed;
//    }

//    private void Update()
//    {
//        if (!playerInRoom) return;

//        HandleStateUpdates();
//        HandleDeathCondition();
//    }

//    private void HandleStateUpdates()
//    {
//        navMeshAgent.destination = _target.position;
//        currentState.UpdateState(this);

//        if (_shouldRotate)
//            RotateTowardsTarget();
//    }

//    private void HandleDeathCondition()
//    {
//        if (enemyHP <= 0)
//        {
//            ResultsMenu.kill_score += 1;
//            resultMenu.SetActive(true);
//            SwichState(deathState);
//        }
//    }

//    private void OnAnimatorMove()
//    {
//        // Полная синхронизация позиции и вращения с анимацией
//        transform.position = animator.rootPosition;
//        transform.rotation = animator.rootRotation;

//        // Корректируем позицию NavMeshAgent
//        navMeshAgent.nextPosition = transform.position;
//    }

//    public void SetSpeed(float newSpeed) => navMeshAgent.speed = newSpeed;
//    public void SetDestination(Transform newDestination) => _target = newDestination;
//    public void SetRotation(bool shouldRotate) => _shouldRotate = shouldRotate;

//    public float DistanceToTarget()
//    {
//        if (_target == null) return Mathf.Infinity;

//        if (NavMesh.CalculatePath(transform.position, _target.position, NavMesh.AllAreas, _cachedPath))
//        {
//            float distance = 0f;
//            for (int i = 1; i < _cachedPath.corners.Length; i++)
//                distance += Vector3.Distance(_cachedPath.corners[i - 1], _cachedPath.corners[i]);
//            return distance;
//        }

//        return Vector3.Distance(transform.position, _target.position);
//    }

//    private void RotateTowardsTarget()
//    {
//        if (_target == null) return;

//        Vector3 direction = (_target.position - transform.position).normalized;
//        direction.y = 0;

//        if (direction != Vector3.zero)
//        {
//            Quaternion targetRotation = Quaternion.LookRotation(direction);
//            transform.rotation = Quaternion.Slerp(
//                transform.rotation,
//                targetRotation,
//                Time.deltaTime * navMeshAgent.angularSpeed / 120f // Динамическая скорость поворота
//            );
//        }
//    }
//}