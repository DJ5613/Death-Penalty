using UnityEngine;
using UnityEngine.AI;

public class EnemyDeath : MonoBehaviour
{
    void FallDestroy()
    {        
        GetComponent<NavMeshAgent>().enabled = false;
        transform.Translate(Vector3.down * 1 * Time.deltaTime, Space.World);
    }
}
