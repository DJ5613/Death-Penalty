using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DamageDetector : MonoBehaviour
{
    [SerializeField] static float playerHP = 100;
    [SerializeField] private TextMeshProUGUI text;

    public void OnDamageDetected(float damage)
    {        
        Debug.Log($"ÿ ñëîâèë {damage} óğîíà");
        playerHP -= damage;
        text.text = "Âàøå õï: " + playerHP.ToString();
        if (playerHP <= 0)
        {
            Debug.Log("ÒÛ ÓÌÅĞ ËÎÕ ÕÀÕÀÕÀÕÀÕÀÕÀÕÕ");
        }
    }
}