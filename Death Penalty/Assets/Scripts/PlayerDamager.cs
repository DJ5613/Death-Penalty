using UnityEngine;

public class PlayerDamager : MonoBehaviour
{
    [SerializeField] public float damage;
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<EnemyStateManager>(out EnemyStateManager manager)) //урон по игроку
        {
            manager.enemyHP -= damage;
            
        }
        else if (other.TryGetComponent<BossStateManager>(out BossStateManager bossManager)) //урон по игроку
        {
            bossManager.enemyHP -= damage;
        }
    }
}
