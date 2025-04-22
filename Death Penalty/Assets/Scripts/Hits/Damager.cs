using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] EnemyStateManager manager;
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DamageDetector>(out DamageDetector detector)) //урон по игроку
        {
            detector.OnDamageDetected(manager.enemyDamage);
        }
    }
}
