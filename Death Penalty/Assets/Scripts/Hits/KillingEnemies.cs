using System;
using UnityEngine;

public class KillingEnemies : MonoBehaviour
{
    [SerializeField] EnemyStateManager manager;
    [NonSerialized] static public float damage = 20f;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            manager.enemyHP -= damage;
            Debug.Log("Враг получил удар оружием! ХП: " + manager.enemyHP);
        }
    }
}
//Крепится на Skeleton/skeleton_mesh
