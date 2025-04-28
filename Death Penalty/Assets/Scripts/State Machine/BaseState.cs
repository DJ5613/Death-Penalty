using UnityEngine;

public abstract class BaseState 
{
    public virtual void EnterState(EnemyStateManager manager) { }
    public virtual void EnterState(BossStateManager manager) { }
    public virtual void ExitState(EnemyStateManager manager) { }
    public virtual void ExitState(BossStateManager manager) { }
    public virtual void UpdateState(EnemyStateManager manager) { }
    public virtual void UpdateState(BossStateManager manager) { }
}
