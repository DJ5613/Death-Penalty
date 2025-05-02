using UnityEngine;

public class SimpleAttackState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
        manager.animator.SetBool("IsSimpleAttacking", true);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.animator.SetBool("IsAgro", false);
        manager.rotationFlag = true;
    }
    public override void UpdateState(EnemyStateManager manager)
    {

    }
}
