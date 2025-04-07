using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    public void OnDamageDetected(EnemyStateManager manager)
    {        
        Debug.Log($"я словил {manager.enemyDamage} урона");        
    }
}