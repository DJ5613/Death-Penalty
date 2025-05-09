using UnityEngine;

public class BuffsLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private string buff;
    public void onGrab()
    {
        switch (buff)
        {
            case "HP":
                DamageDetector.playerHP += 100;
                Destroy(gameObject);
                break;
            case "Damage":
                KillingEnemies.damage += 5f;
                Destroy(gameObject);
                break;
            case "Zamedlo":
                //EnemyStateManager.wolkSpeed *= 0.8f;
                Destroy(gameObject);
                break;
        }
    }

}
