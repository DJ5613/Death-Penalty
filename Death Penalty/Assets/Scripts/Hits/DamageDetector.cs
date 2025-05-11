using TMPro;
using UnityEngine;

public class DamageDetector : MonoBehaviour
{
    [SerializeField] static public float playerHP = 100;
    [SerializeField] private GameObject resultMenu;


    private void Start()
    {
        playerHP = 100;
    }
    public void OnDamageDetected(float damage)
    {        
        Debug.Log($"ÿ ñëîâèë {damage} óğîíà");
        playerHP -= damage;
        if (playerHP <= 0)
        {
            Debug.Log("ÒÛ ÓÌÅĞ ËÎÕ ÕÀÕÀÕÀÕÀÕÀÕÀÕÕ");
            resultMenu.SetActive(true);
        }
    }
}