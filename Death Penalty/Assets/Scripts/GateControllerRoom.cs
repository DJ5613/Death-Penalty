using System.Collections.Generic;
using UnityEngine;

public class GateControllerRoom : MonoBehaviour
{
    public Transform gate1;
    public Transform gate2;
    public Vector3 raisedOffset = new Vector3(0, 5.65f, 0); // Смещение для поднятых ворот
    public float moveSpeed = 2.5f;
    public string enemyTag = "skeleton";

    [SerializeField] private GameObject[] buffs;

    private enum GateState { Open, Opening, Closed, Closing }
    private GateState gateState = GateState.Open; // Начинаем с ОТКРЫТЫХ ворот (опущенных)

    private Vector3 gate1OpenPos, gate1ClosedPos;
    private Vector3 gate2OpenPos, gate2ClosedPos;

    private bool playerInside = false;
    private HashSet<GameObject> enemiesInRoom = new HashSet<GameObject>();

    void Start()
    {
        // Текущая позиция - это открытое (опущенное) положение
        gate1OpenPos = gate1.position;
        // Закрытое положение - поднятое (текущая позиция + смещение)
        gate1ClosedPos = gate1OpenPos + raisedOffset;

        gate2OpenPos = gate2.position;
        gate2ClosedPos = gate2OpenPos + raisedOffset;
    }

    void Update()
    {
        switch (gateState)
        {
            case GateState.Opening:
                // ОТКРЫВАЕМ (опускаем ворота)
                MoveGate(gate1, gate1OpenPos);
                MoveGate(gate2, gate2OpenPos);

                if (IsAtPosition(gate1, gate1OpenPos) && IsAtPosition(gate2, gate2OpenPos))
                {
                    gateState = GateState.Open; // Полностью открыто
                }
                break;

            case GateState.Closing:
                // ЗАКРЫВАЕМ (поднимаем ворота)
                MoveGate(gate1, gate1ClosedPos);
                MoveGate(gate2, gate2ClosedPos);

                if (IsAtPosition(gate1, gate1ClosedPos) && IsAtPosition(gate2, gate2ClosedPos))
                {
                    gateState = GateState.Closed; // Полностью закрыто
                }
                break;

            case GateState.Closed:
                
                if (enemiesInRoom.Count == 0)
                {
                    gameObject.GetComponent<SpawnBuffs>().SpawnBuffNow();
                    gateState = GateState.Opening;
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
                // Если ворота открыты - начинаем закрывать
                if (gateState == GateState.Open)
                {
                    gateState = GateState.Closing;
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