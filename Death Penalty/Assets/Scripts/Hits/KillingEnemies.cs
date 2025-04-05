using UnityEngine;

public class KillingEnemies : MonoBehaviour
{
    public int HP = 3;
    private int HPCount = 0;

    private void OnTriggerEnter(Collider other)
    {
            if (other.CompareTag("Weapon"))
            {
                HPCount++;

                Debug.Log("Враг получил удар оружием! Счётчик: " + HPCount);

                if (HPCount >= HP)
                {
                    Destroy(gameObject);
                    Debug.Log("Враг убит!");
                }
            }
        
    }

}
