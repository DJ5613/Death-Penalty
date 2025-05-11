using UnityEngine;

public class BossAgroState : BaseState
{
    public override void EnterState(BossStateManager manager)
    {
        Debug.Log("онкмппнолюнопюлопнлоплнпнколплон");
        manager.animator.SetBool("IsAgro", true);
        manager.animator.SetBool("IsUndercuting", false);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.animator.SetBool("IsDownAttacking", false);
        manager.animator.SetBool("IsTired", false);
    }
    public override void UpdateState(BossStateManager manager)
    {
        //if (manager.DistanceToTarget() >= manager.agroDistance)
        //{
        //    manager.SwichState(manager.idleState);
        //    return;
        //}

        if (manager.DistanceToTarget() > manager.simpleAttackDistance)
        {
            manager.SwichState(manager.bossUndercutState);
            return;
        }
    }
}

public class BossComboState : BaseState
{
    public override void EnterState(BossStateManager manager)
    {
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsUndercuting", false);
        manager.animator.SetBool("IsComboAttacking", true);
        manager.animator.SetBool("IsDownAttacking", false);
        manager.animator.SetBool("IsTired", false);
    }
    public override void UpdateState(BossStateManager manager)
    {
        //if (manager.DistanceToTarget() >= manager.agroDistance)
        //{
        //    manager.SwichState(manager.bossComboState);
        //    return;
        //}

        //if (manager.DistanceToTarget() <= manager.comboAttackDistance)
        //{
        //    manager.SwichState(manager.comboAttackState);
        //    return;
        //}
    }
}

public class BossDeathState : BaseState
{
    public override void EnterState(BossStateManager manager)
    {
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsUndercuting", false);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.animator.SetBool("IsDownAttacking", false);
        manager.animator.SetBool("IsTired", false);
        manager.animator.SetBool("IsDeath", true) ;
    }
    public override void UpdateState(BossStateManager manager)
    {
        //if (manager.DistanceToTarget() >= manager.agroDistance)
        //{
        //    manager.SwichState(manager.bossComboState);
        //    return;
        //}

        //if (manager.DistanceToTarget() <= manager.comboAttackDistance)
        //{
        //    manager.SwichState(manager.comboAttackState);
        //    return;
        //}
    }
}

public class BossUndercutState : BaseState
{
    public override void EnterState(BossStateManager manager)
    {
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsUndercuting", true);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.animator.SetBool("IsDownAttacking", false);
        manager.animator.SetBool("IsTired", false);
    }
    public override void UpdateState(BossStateManager manager)
    {
        //if (manager.DistanceToTarget() >= manager.agroDistance)
        //{
        //    manager.SwichState(manager.bossComboState);
        //    return;
        //}

        //if (manager.DistanceToTarget() <= manager.comboAttackDistance)
        //{
        //    manager.SwichState(manager.bossComboState);
        //    return;
        //}
    }
}

public class BossDownAttackState : BaseState
{
    public override void EnterState(BossStateManager manager)
    {
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsUndercuting", false);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.animator.SetBool("IsDownAttacking", true);
        manager.animator.SetBool("IsTired", false);
    }
    public override void UpdateState(BossStateManager manager)
    {
        //if (manager.DistanceToTarget() >= manager.agroDistance)
        //{
        //    manager.SwichState(manager.bossComboState);
        //    return;
        //}

        //if (manager.DistanceToTarget() <= manager.comboAttackDistance)
        //{
        //    manager.SwichState(manager.bossComboState);
        //    return;
        //}
    }
}