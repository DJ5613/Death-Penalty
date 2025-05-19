using System;
using UnityEngine;

public class KillingEnemies : MonoBehaviour
{
    [SerializeField] EnemyStateManager manager;
    [SerializeField] BossStateManager bossManager;
    static public float damage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            if (manager != null)
            {
                manager.enemyHP -= damage;
                Debug.Log("Враг получил удар оружием! ХП: " + manager.enemyHP);
                manager.PlayHitSound();
            }
            else
            {
                bossManager.enemyHP -= damage;
                Debug.Log("Враг получил удар оружием! ХП: " + bossManager.enemyHP);
            }
        }
    }
}
//Крепится на Skeleton/skeleton_mesh
