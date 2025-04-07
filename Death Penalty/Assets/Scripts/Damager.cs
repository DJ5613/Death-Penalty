using UnityEngine;

public class Damager : MonoBehaviour
{
    public void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<DamageDetector>(out DamageDetector detector))
        {
            detector.OnDamageDetected(GetComponent<EnemyStateManager>());
        }
    }
}
