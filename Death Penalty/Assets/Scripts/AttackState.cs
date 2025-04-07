using UnityEngine;

public class SimpleAttackState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
        manager.animator.SetBool("IsSimpleAttacking", true);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.animator.SetBool("IsAgro", false);
    }
    public override void ExitState(EnemyStateManager manager)
    {

    }
    public override void UpdateState(EnemyStateManager manager)
    {
        //if (manager.DistanceToTarget() > manager.attackDistance)
        //{
        //    manager.SwichState(manager.agroState);
        //    return;
        //}
    }
}
