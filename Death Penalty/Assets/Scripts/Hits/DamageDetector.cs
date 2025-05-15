using UnityEngine;

public class DamageDetector : MonoBehaviour
{    
    [SerializeField] private GameObject resultMenu;
    public static float playerHP;
    public static float maxPlayerHP;
    public static float PlayerHP
    {
        get => playerHP;
        set
        {
            if (value > maxPlayerHP) playerHP = maxPlayerHP;
            else if (value<= 0) playerHP = 0;
            else playerHP = value;
        }
    }


    private void Start()
    {
        playerHP = 100;
        maxPlayerHP = playerHP;
    }
    public void OnDamageDetected(float damage)
    {        
        Debug.Log($"ÿ ñëîâèë {damage} óðîíà");
        playerHP -= damage;
        if (playerHP <= 0)
        {
            Debug.Log("ÒÛ ÓÌÅÐ ËÎÕ ÕÀÕÀÕÀÕÀÕÀÕÀÕÕ");
            resultMenu.SetActive(true);
        }
    }
}