using UnityEngine;

public class BreakableObject : MonoBehaviour
{
    public int hitsToDestroy = 3;
    private int hitCount = 0;

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Weapon"))
        {
            hitCount++;

            Debug.Log("Объект получил удар оружием! Счётчик: " + hitCount);

            if (hitCount >= hitsToDestroy)
            {
                Destroy(gameObject);
                Debug.Log("Объект разрушен!");
            }
        }
    }
}
