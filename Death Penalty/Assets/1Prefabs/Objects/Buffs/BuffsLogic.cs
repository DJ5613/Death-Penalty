using UnityEngine;

public class BuffsLogic : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private string buff;

    void onGrab()
    {
        switch (buff)
        {
            case "HP":
                break;
            case "Damage":
                break;
            case "Zamedlo":
                break;
        }
    }

}
