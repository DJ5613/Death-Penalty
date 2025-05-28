using Unity.VisualScripting;
using UnityEngine;

public class KrytoiDamage : MonoBehaviour
{
    public float damage = 20f;
    public float minHitSpeed = 5f; // Минимальная скорость удара

    public static float damageBuff = 1f;

    private Vector3 lastPosition;
    private float currentSpeed;

    void Update()
    {
        // Считаем скорость меча вручную
        currentSpeed = (transform.position - lastPosition).magnitude / Time.deltaTime;
        lastPosition = transform.position;
    }

    void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "Object") return;

        if (currentSpeed < minHitSpeed)
        {
            Debug.Log("Слабо");
            return;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            if (collision.gameObject.TryGetComponent<EnemyStateManager>(out EnemyStateManager manager)) //урон по врагу
            {
                manager.enemyHP -= (damage*damageBuff);
                Debug.Log("Враг получил удар оружием! ХП: " + manager.enemyHP);
                manager.PlayHitSound();
            }
        }

        if (collision.gameObject.tag == "Object")
        {
            if (collision.gameObject.TryGetComponent<BreakableObject>(out BreakableObject obj)) //Урон по объекту
            {
                obj.TakeHit();
            }
        }
    }
}