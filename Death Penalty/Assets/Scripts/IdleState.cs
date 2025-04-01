using UnityEngine;

public class IdleState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
    }
    public override void ExitState(EnemyStateManager manager)
    {   

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        if (manager.DistanceToTarget() < manager.agroDistance) manager.SwichState(manager.agroState);
    }
}
