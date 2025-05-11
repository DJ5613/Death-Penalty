using System.Collections.Generic;
using UnityEngine;

public class GateControllerRoom : MonoBehaviour
{
    public Transform gate1;
    public Transform gate2;
    public Vector3 raisedOffset = new Vector3(0, 5.65f, 0);
    public float moveSpeed = 2.5f;
    public string enemyTag = "skeleton";

    [SerializeField] private GameObject[] buffs;

    private enum GateState { Closed, Opening, Opened, Closing }
    private GateState gateState = GateState.Closed;

    private Vector3 gate1ClosedPos, gate1OpenPos;
    private Vector3 gate2ClosedPos, gate2OpenPos;

    private bool playerInside = false;
    private HashSet<GameObject> enemiesInRoom = new HashSet<GameObject>();

    void Start()
    {
        gate1ClosedPos = gate1.position;
        gate1OpenPos = gate1ClosedPos + raisedOffset;

        gate2ClosedPos = gate2.position;
        gate2OpenPos = gate2ClosedPos + raisedOffset;

    }

    void Update()
    {
        switch (gateState)
        {
            case GateState.Opening:
                MoveGate(gate1, gate1OpenPos);
                MoveGate(gate2, gate2OpenPos);

                if (IsAtPosition(gate1, gate1OpenPos) && IsAtPosition(gate2, gate2OpenPos))
                {
                    gateState = GateState.Opened;
                }
                break;

            case GateState.Closing:
                MoveGate(gate1, gate1ClosedPos);
                MoveGate(gate2, gate2ClosedPos);

                if (IsAtPosition(gate1, gate1ClosedPos) && IsAtPosition(gate2, gate2ClosedPos))
                {
                    gateState = GateState.Closed;
                }
                break;

            case GateState.Opened:
                if (enemiesInRoom.Count == 0)
                {
                    gameObject.GetComponent<SpawnBuffs>().SpawnBuffNow();
                    gateState = GateState.Closing;

                }
                break;
        }
    }

    void MoveGate(Transform gate, Vector3 targetPos)
    {
        gate.position = Vector3.MoveTowards(gate.position, targetPos, moveSpeed * Time.deltaTime);
    }

    bool IsAtPosition(Transform gate, Vector3 targetPos)
    {
        return Vector3.Distance(gate.position, targetPos) < 0.01f;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!playerInside)
            {
                playerInside = true;
                if (gateState == GateState.Closed)
                {
                    gateState = GateState.Opening;
                }
            }
        }

        if (other.CompareTag(enemyTag))
        {
            enemiesInRoom.Add(other.gameObject);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(enemyTag))
        {
            // Debug.Log("FSDFSDFGSDFS");
            ResultsMenu.kill_score += 1;
            enemiesInRoom.Remove(other.gameObject);
        }
    }

    public void RemoveEnemy(GameObject enemy)
    {
        enemiesInRoom.Remove(enemy);
    }

    public void SpawnBuff()
    {
        int randomIndex = Random.Range(0, buffs.Length);

    }
}
