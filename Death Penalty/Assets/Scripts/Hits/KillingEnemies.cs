//using System;
//using UnityEngine;
//using UnityEngine.InputSystem.XR.Haptics;

//public class KillingEnemies : MonoBehaviour
//{
//    [SerializeField] EnemyStateManager manager;
//    [SerializeField] BossStateManager bossManager;
//    static public float damage = 10f;

//    private void OnTriggerEnter(Collider other)
//    {
//        if (other.CompareTag("Weapon"))
//        {
//            if (manager != null)
//            {
//                manager.enemyHP -= damage;
//                Debug.Log("Враг получил удар оружием! ХП: " + manager.enemyHP);
//                manager.PlayHitSound();
                
//            }
//            else
//            {
//                bossManager.enemyHP -= damage;
//                Debug.Log("Враг получил удар оружием! ХП: " + bossManager.enemyHP);
//            }
//        }
//    }
//}
