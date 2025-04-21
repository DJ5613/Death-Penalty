using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    [SerializeField] float playerHP = 100;
    
    public void OnDamageDetected(float damage)
    {        
        Debug.Log($"ÿ ñëîâèë {damage} óğîíà");
        playerHP -= damage;
        if (playerHP <= 0) Debug.Log("ÒÛ ÓÌÅĞ ÍÀÕÓÉ ËÎÕ ÕÀÕÀÕÀÕÀÕÀÕÀÕÕ");
    }
}