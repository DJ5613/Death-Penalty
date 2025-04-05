using UnityEngine;

public class GateController : MonoBehaviour
{
    public Transform gate;
    public Vector3 raisedPositionOffset = new Vector3(0, 5f, 0);
    public float moveSpeed = 2f;
    public string enemyTag = "Enemy";

    private Vector3 loweredPosition;
    private Vector3 raisedPosition;

    private bool playerInside = false;
    private bool isGateUp = false;
    private bool allEnemiesCleared = false;

    void Start()
    {
        if (gate == null)
        {
            Debug.LogError("Gate not assigned!");
            return;
        }

        loweredPosition = gate.position;
        raisedPosition = loweredPosition + raisedPositionOffset;
    }

    void Update()
    {
        if (playerInside && !isGateUp)
        {
            gate.position = Vector3.MoveTowards(gate.position, raisedPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(gate.position, raisedPosition) < 0.01f)
            {
                isGateUp = true;
                Debug.Log("Ворота подняты");
            }
        }
        else if (isGateUp && !allEnemiesCleared && NoEnemiesLeft())
        {
            Debug.Log("Врагов больше нет — опускаем ворота");
            allEnemiesCleared = true;
        }

        if (allEnemiesCleared && gate.position != loweredPosition)
        {
            gate.position = Vector3.MoveTowards(gate.position, loweredPosition, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(gate.position, loweredPosition) < 0.01f)
            {
                isGateUp = false;
                Debug.Log("Ворота опущены");
            }
        }
    }

    bool NoEnemiesLeft()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        return enemies.Length == 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            Debug.Log("Игрок зашёл в комнату");
        }
    }
}
