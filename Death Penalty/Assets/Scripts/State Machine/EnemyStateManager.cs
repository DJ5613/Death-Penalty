using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    [SerializeField] public Animator animator;
    [SerializeField] GameObject sword;
    public float wolkSpeed;
    public float agroDistance;
    public float simpleAttackDistance;
    public float comboAttackDistance;
    public float comboAttackSpeed;
    public float enemyHP;
    public float enemyDamage;
    public bool rotationFlag = true;
    [NonSerialized] static public float slow = 1;
    Transform target;
    NavMeshPath _cachedPath;

    BaseState currentState;
    public IdleState idleState = new IdleState();
    public AgroState agroState = new AgroState();
    public SimpleAttackState simpleAttackState = new SimpleAttackState();
    public ComboAttackState comboAttackState = new ComboAttackState();
    public DeathState deathState = new DeathState();
    public static bool playerInRoom = true; //Должно быть false, пока что сделано для теста
    public void SwichState(BaseState newState)
    {
        if (currentState != null)
        {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);//не используется
    }

    private void Start()
    {
        switch (SwitchDifficulty.dif_num)
        {
            case 1:
                wolkSpeed = (float)(wolkSpeed * 0.7);
                enemyHP = (float)(enemyHP * 0.5);
                enemyDamage = (float)(enemyDamage * 0.5);
                break;
            case 2:
                break;
            case 3:
                wolkSpeed = (float)(wolkSpeed * 1.2);
                enemyHP = (float)(enemyHP * 1.5);
                enemyDamage = (float)(enemyDamage * 1.5);
                break;
        }
        if (player == null) player = GameObject.FindGameObjectWithTag("Player").transform;
        _cachedPath = new NavMeshPath();
        SwichState(idleState);
        
    }
    private void Update()
    {        
        if (playerInRoom)
        {
            wolkSpeed *= slow;
            SetDestination(player);
            navMeshAgent.destination = target.position;
            currentState.UpdateState(this);
            if (DistanceToTarget() < agroDistance && !animator.GetBool("IsDeath") && rotationFlag) RotateTowardsTarget();
        }
        if (enemyHP <= 0) 
        { 
            SwichState(deathState);
            sword.transform.SetParent(null);
            sword.GetComponent<Rigidbody>().isKinematic = false;
            sword.GetComponent<Collider>().isTrigger = false;
            transform.Translate(Vector3.down * 1 * Time.deltaTime, Space.World);
            //rb.useGravity = true;
            
            Debug.Log("ВРАГ УМЕР");
            Collider collider = GetComponent<Collider>();
            collider.enabled = false;
            BoxCollider boxCollider = GetComponent<BoxCollider>();
            boxCollider.center -= new Vector3(0,5,0);
        }
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
    void IsRotate(int value) 
    { 
        if (value == 1) rotationFlag = true;
        else rotationFlag = false;
    }
}
