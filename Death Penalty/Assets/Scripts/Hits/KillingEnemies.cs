using UnityEngine;

public class KillingEnemies : MonoBehaviour
{
    [SerializeField] EnemyStateManager manager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            manager.enemyHP -= 20f;
            Debug.Log("Враг получил удар оружием! ХП: " + manager.enemyHP);
        }
    }
}
//Крепится на Skeleton/skeleton_mesh
