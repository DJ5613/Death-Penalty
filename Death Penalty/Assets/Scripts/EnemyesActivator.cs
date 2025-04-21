using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Comfort;

public class EnemyesActivator : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!EnemyStateManager.playerInRoom)
            {
                EnemyStateManager.playerInRoom = true;
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            EnemyStateManager.playerInRoom = false;
        }
    }
}
