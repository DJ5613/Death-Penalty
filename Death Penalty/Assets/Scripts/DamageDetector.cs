using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    
    public void OnDamageDetected(float damage)
    {        
        Debug.Log($"я словил {damage} урона");        
    }
}