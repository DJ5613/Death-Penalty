using Unity.VisualScripting;
using UnityEngine;

public class KrytoiDamage : MonoBehaviour
{
    public float damage = 20f;
    public float minHitSpeed = 5f; // Минимальная скорость удара

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
        if (currentSpeed < minHitSpeed)
        {
            Debug.Log("Слабо");
            return;
        }

            if (collision.gameObject.tag != "Enemy") return;

        if (collision.gameObject.TryGetComponent<EnemyStateManager>(out EnemyStateManager manager)) //урон по врагу
        {
            manager.enemyHP -= damage;
            Debug.Log("Враг получил удар оружием! ХП: " + manager.enemyHP);
            manager.PlayHitSound();
        }
    }
}