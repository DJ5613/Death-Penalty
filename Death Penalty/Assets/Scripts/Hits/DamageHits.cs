using UnityEngine;

public class DamageHits : MonoBehaviour
{
    public int HP = 5;
    private int Damage = 1;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Weapon"))
        {
            HP-=Damage;
            Debug.Log("Враг получил удар оружием! Счётчик: " + HP);
            if (HP <= 0)
            {
                Destroy(gameObject);
                Debug.Log("Враг убит!");
            }
        }
    }
}
