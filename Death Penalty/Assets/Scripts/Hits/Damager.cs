using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] EnemyStateManager manager;
    [SerializeField] BossStateManager bossManager;
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DamageDetector>(out DamageDetector detector)) //урон по игроку
        {
            if (manager!= null) detector.OnDamageDetected(manager.enemyDamage);
            else detector.OnDamageDetected(bossManager.enemyDamage);
        }
    }
}
