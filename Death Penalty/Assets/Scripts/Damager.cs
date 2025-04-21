using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] public float damage;
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DamageDetector>(out DamageDetector detector)) //урон по игроку
        {
            detector.OnDamageDetected(damage);
        }
    }
}
