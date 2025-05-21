using UnityEngine;

public class BossActivator : MonoBehaviour
{
    [SerializeField] BossStateManager bossManager;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!BossStateManager.playerInRoom)
            {
                BossStateManager.playerInRoom = true;
                //bossManager.animator.SetBool("IsAgro", true);
                bossManager.SwichState(bossManager.bossAgroState);
            }
        }
    }
}