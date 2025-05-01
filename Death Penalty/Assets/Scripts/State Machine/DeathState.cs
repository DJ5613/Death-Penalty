using UnityEngine;

public class DeathState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
        manager.animator.SetBool("IsDeath", true);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.animator.SetBool("IsSimpleAttacking", false);
    }
    public override void ExitState(EnemyStateManager manager)
    {

    }
    public override void UpdateState(EnemyStateManager manager)
    {

    }
}
