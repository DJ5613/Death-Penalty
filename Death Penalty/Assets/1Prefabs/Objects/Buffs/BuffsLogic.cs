using UnityEngine;

public class BuffsLogic : MonoBehaviour
{

    [SerializeField] private string buff;
    public void onGrab()
    {
        switch (buff)
        {
            case "HP":
                DamageDetector.maxPlayerHP += 100;
                DamageDetector.playerHP = DamageDetector.maxPlayerHP;
                Destroy(gameObject);
                break;
            case "Damage":
                //KillingEnemies.damage *= 1.2f;
                Destroy(gameObject);
                break;
            case "Zamedlo":
                EnemyStateManager.slow = 0.8f;
                Destroy(gameObject);
                break;
        }
    }
}
