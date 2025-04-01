using UnityEngine;
using UnityEngine.AI;

public class EnemyStateManager : MonoBehaviour
{
    [SerializeField] NavMeshAgent navMeshAgent;
    [SerializeField] Transform player;
    [SerializeField] public float wolkSpeed;
    [SerializeField] public float agroDistance;
    [SerializeField] public float attackDistance;
    Transform target;

    BaseState currentState;
    public IdleState idleState = new IdleState();
    public AgroState agroState = new AgroState();
    public AttackState attackState = new AttackState();
     
    public void SwichState(BaseState newState)
    {
        if (currentState != null) {
            currentState.ExitState(this);
        }
        currentState = newState;
        currentState.EnterState(this);
    }

    private void Start()
    {
        SwichState(idleState);
    }
    private void Update()
    {
        SetDestination(player);
        navMeshAgent.destination = target.position;  
        currentState.UpdateState(this);
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
        return(transform.position - target.transform.position).magnitude;        
    }
}
