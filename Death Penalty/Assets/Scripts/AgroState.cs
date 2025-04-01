using UnityEngine;

public class AgroState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(manager.wolkSpeed);
    }
    public override void ExitState(EnemyStateManager manager)
    {

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        if (manager.DistanceToTarget() >= manager.agroDistance)
        {
            manager.SwichState(manager.idleState);
            return;
        }

        if (manager.DistanceToTarget()  <= manager.attackDistance)
        {
            manager.SwichState(manager.attackState);
            return;
        }
    }
}
