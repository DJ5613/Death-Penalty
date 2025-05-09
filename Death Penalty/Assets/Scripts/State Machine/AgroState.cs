using UnityEngine;

public class AgroState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.animator.SetBool("IsAgro", true);
        manager.animator.SetBool("IsSimpleAttacking", false);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.SetSpeed(EnemyStateManager.wolkSpeed);
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

        if (manager.DistanceToTarget()  <= manager.comboAttackDistance)
        {
            manager.SwichState(manager.comboAttackState);
            return;
        }
    }
}
