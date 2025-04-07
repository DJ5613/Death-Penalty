using UnityEngine;

public class KillingEnemies : MonoBehaviour
{
    public int HP = 3;
    private int HPCount = 0;

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Weapon"))
            {
                HP -= other.gameObject.GetComponent<WeaponStats>().damage;
                

                Debug.Log("Враг получил удар оружием! ХП: " + HP);

                if (HP<=0)
                {
                    Destroy(gameObject);
                    Debug.Log("Враг убит!");
                }
            }
        
    }

}
