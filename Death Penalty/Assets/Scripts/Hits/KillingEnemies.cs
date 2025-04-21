using UnityEngine;

public class KillingEnemies : MonoBehaviour
{
    [SerializeField] static EnemyStateManager manager;
    private  float HP = manager.enemyHP;

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Weapon"))
            {
            //HP -= other.gameObject.GetComponent<WeaponStats>().damage;
            manager.enemyHP -= 20f;
                

                Debug.Log("Враг получил удар оружием! ХП: " + manager.enemyHP);

                //if (HP<=0)
                //{
                //    Destroy(gameObject);
                //    Debug.Log("Враг убит!");
                //}
            }
        
    }

}
