using GLTFast.Schema;
using UnityEngine;

public class BossAgroState : BaseState
{
    public override void EnterState(BossStateManager manager)
    {
        //Debug.Log("агро");
        manager.animator.SetBool("IsAgro", true);
        manager.animator.SetBool("IsComboAttacking", false);
        //manager.animator.SetBool("IsTired", false);
        //manager.isRotate = true;
    }
    public override void UpdateState(BossStateManager manager)
    {
        if (manager.DistanceToTarget() <= manager.simpleAttackDistance)
        {
            manager.SwichState(manager.bossAttackState);
            return;
        }
        if (((manager.DistanceToTarget() > (manager.comboAttackDistance- manager.comboAttackRange))) 
            && (manager.DistanceToTarget() <= manager.comboAttackDistance) && manager.animator.GetBool("SecondStage"))
        {
            manager.SwichState(manager.bossComboState);
            manager.isRotate = false;
            return;
        }              
    }
}

public class BossComboState : BaseState
{
    public override void EnterState(BossStateManager manager)
    {
        Debug.Log("комбо");
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsComboAttacking", true);
        manager.animator.SetBool("IsTired", false);
        Debug.Log(manager.AttackNum);
    }
    public override void UpdateState(BossStateManager manager)
    {
        if (manager.DistanceToTarget() <= manager.simpleAttackDistance)
        {
            manager.SwichState(manager.bossAttackState);
            return;
        }
        else if ((manager.DistanceToTarget() > manager.simpleAttackDistance) && (manager.DistanceToTarget()
            < (manager.comboAttackDistance - manager.comboAttackRange)) || manager.DistanceToTarget() > manager.comboAttackDistance)
        {
            manager.SwichState(manager.bossAgroState);
            return;
        }
    }
}

public class BossAttackState : BaseState
{
    public override void EnterState(BossStateManager manager)
    {
        //Debug.Log("атака");
        manager.animator.SetBool("IsAgro", false);
        manager.animator.SetBool("IsComboAttacking", false);
        manager.animator.SetBool("IsTired", false);
        //if (manager.AttackNum == manager.MaxAttackNum) manager.animator.SetBool("IsTired", true);
        //Debug.Log(manager.AttackNum);
    }
    public override void UpdateState(BossStateManager manager)
    {
        if ((manager.DistanceToTarget() > manager.simpleAttackDistance) && (manager.DistanceToTarget()
            < manager.comboAttackDistance - manager.comboAttackRange))
        {
            manager.SwichState(manager.bossAgroState);
            return;
        }
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
        manager.isRotate = false;
    }
}


//public class BossUndercutState : BaseState
//{
//    public override void EnterState(BossStateManager manager)
//    {
//        manager.animator.SetBool("IsAgro", false);
//        manager.animator.SetBool("IsUndercuting", true);
//        manager.animator.SetBool("IsComboAttacking", false);
//        manager.animator.SetBool("IsDownAttacking", false);
//        manager.animator.SetBool("IsTired", false);
//    }
//    public override void UpdateState(BossStateManager manager)
//    {
//        //if (manager.DistanceToTarget() >= manager.agroDistance)
//        //{
//        //    manager.SwichState(manager.bossComboState);
//        //    return;
//        //}

//        //if (manager.DistanceToTarget() <= manager.comboAttackDistance)
//        //{
//        //    manager.SwichState(manager.bossComboState);
//        //    return;
//        //}
//    }
//}

//public class BossDownAttackState : BaseState
//{
//    public override void EnterState(BossStateManager manager)
//    {
//        manager.animator.SetBool("IsAgro", false);
//        manager.animator.SetBool("IsUndercuting", false);
//        manager.animator.SetBool("IsComboAttacking", false);
//        manager.animator.SetBool("IsDownAttacking", true);
//        manager.animator.SetBool("IsTired", false);
//    }
//    public override void UpdateState(BossStateManager manager)
//    {
//        //if (manager.DistanceToTarget() >= manager.agroDistance)
//        //{
//        //    manager.SwichState(manager.bossComboState);
//        //    return;
//        //}

//        //if (manager.DistanceToTarget() <= manager.comboAttackDistance)
//        //{
//        //    manager.SwichState(manager.bossComboState);
//        //    return;
//        //}
//    }
//}

