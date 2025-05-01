using UnityEngine;

public class ComboAttackState : BaseState
{
    public override void EnterState(EnemyStateManager manager)
    {
        manager.SetSpeed(0);
        manager.animator.SetBool("IsSimpleAttacking", false);
        manager.animator.SetBool("IsComboAttacking", true);
        manager.animator.SetBool("IsAgro", false);
    }
  
    public override void UpdateState(EnemyStateManager manager)
    {

    }

    
}
