using System.Collections.Generic;
using UnityEngine;

public class DungeonGenerator : MonoBehaviour
{
    [Header("Основные настройки")]
    [Tooltip("Минимальное количество комнат (включая старт и босса)")]
    public int minRooms = 5;
    [Tooltip("Максимальное количество комнат (включая старт и босса)")]
    public int maxRooms = 10;
    [Tooltip("Расстояние между центрами комнат")]
    public float cellSize = 20f;
    [Tooltip("Фактический размер комнат (должен быть меньше cellSize)")]
    public float roomSize = 15f;
    [Tooltip("Ширина коридоров")]
    public float corridorWidth = 3f;

    [Header("Префабы")]
    public GameObject startRoomPrefab;
    public List<GameObject> roomPrefabs;
    public GameObject bossRoomPrefab;
    public GameObject corridorWallPrefab;
    [Tooltip("Опционально")]
    public GameObject treasureRoomPrefab;
    [Tooltip("Опционально")]
    public GameObject secretRoomPrefab;

    private HashSet<Vector2Int> spawnedRoomsGrid = new HashSet<Vector2Int>();
    private Dictionary<Vector2Int, GameObject> spawnedRooms = new Dictionary<Vector2Int, GameObject>();
    private List<Vector2Int> directions = new List<Vector2Int>
    {
        Vector2Int.up,    // Z+
        Vector2Int.right, // X+
        Vector2Int.down,  // Z-
        Vector2Int.left   // X-
    };

    void Start()
    {
        GenerateDungeon();
    }

    void GenerateDungeon()
    {
        // Очистка предыдущего подземелья
        ClearPreviousDungeon();

        // 1. Стартовая комната
        Vector2Int startPos = Vector2Int.zero;
        SpawnRoom(startPos, startRoomPrefab);

        // 2. Основные комнаты
        int roomsToGenerate = Random.Range(minRooms, maxRooms + 1);
        Vector2Int currentPos = startPos;
        Vector2Int previousPos = startPos;

        for (int i = 1; i < roomsToGenerate - 1; i++)
        {
            Vector2Int newPos = FindNextValidPosition(currentPos);

            if (newPos == currentPos)
            {
                newPos = GetRandomAdjacentPosition(currentPos);
                if (newPos == currentPos) break;
            }

            SpawnRoom(newPos, GetRandomRoomPrefab(false));
            CreateCorridor(previousPos, currentPos);

            previousPos = currentPos;
            currentPos = newPos;
        }

        // 3. Комната босса
        Vector2Int bossPos = FindFarthestPosition();
        SpawnRoom(bossPos, bossRoomPrefab);
        CreateCorridor(currentPos, bossPos);

        Debug.Log($"Сгенерировано подземелье! Комнат: {spawnedRoomsGrid.Count}");
    }

    Vector2Int FindNextValidPosition(Vector2Int currentPos)
    {
        List<Vector2Int> shuffledDirs = new List<Vector2Int>(directions);
        ShuffleList(shuffledDirs);

        foreach (Vector2Int dir in shuffledDirs)
        {
            Vector2Int newPos = currentPos + dir;
            if (!spawnedRoomsGrid.Contains(newPos))
                return newPos;
        }

        return currentPos;
    }

    Vector2Int GetRandomAdjacentPosition(Vector2Int pos)
    {
        List<Vector2Int> availablePositions = new List<Vector2Int>();

        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = pos + dir;
            if (!spawnedRoomsGrid.Contains(newPos))
                availablePositions.Add(newPos);
        }

        return availablePositions.Count > 0 ?
            availablePositions[Random.Range(0, availablePositions.Count)] :
            pos;
    }

    Vector2Int FindFarthestPosition()
    {
        Vector2Int farthestPos = Vector2Int.zero;
        int maxDistance = 0;

        foreach (Vector2Int pos in spawnedRoomsGrid)
        {
            int distance = Mathf.Abs(pos.x) + Mathf.Abs(pos.y);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestPos = pos;
            }
        }

        // Пытаемся найти свободное место рядом
        foreach (Vector2Int dir in directions)
        {
            Vector2Int newPos = farthestPos + dir;
            if (!spawnedRoomsGrid.Contains(newPos))
                return newPos;
        }

        return farthestPos;
    }

    void CreateCorridor(Vector2Int from, Vector2Int to)
    {
        Vector3 start = GridToWorld(from);
        Vector3 end = GridToWorld(to);
        Vector3 direction = (end - start).normalized;

        bool isHorizontal = Mathf.Abs(direction.x) > 0;
        float corridorLength = Vector3.Distance(start, end) - roomSize;
        Vector3 corridorCenter = (start + end) * 0.5f;

        GameObject corridor = new GameObject("Corridor");
        corridor.transform.position = corridorCenter;
        corridor.transform.rotation = isHorizontal ? Quaternion.Euler(0, 90, 0) : Quaternion.identity;
        corridor.transform.parent = transform;

        // Правая стенка
        CreateWall(corridor.transform, corridorWidth / 2, corridorLength);

        // Левая стенка
        CreateWall(corridor.transform, -corridorWidth / 2, corridorLength);
    }

    void CreateWall(Transform parent, float xOffset, float length)
    {
        GameObject wall = Instantiate(corridorWallPrefab, parent);
        wall.transform.localPosition = new Vector3(xOffset, 0, 0);
        wall.transform.localScale = new Vector3(
            0.3f, // Толщина стенки
            3f,   // Высота стенки
            length // Длина коридора
        );
    }

    void SpawnRoom(Vector2Int gridPos, GameObject prefab)
    {
        Vector3 worldPos = GridToWorld(gridPos);
        GameObject room = Instantiate(prefab, worldPos, Quaternion.identity, transform);

        spawnedRoomsGrid.Add(gridPos);
        spawnedRooms[gridPos] = room;
    }

    GameObject GetRandomRoomPrefab(bool isBossRoom)
    {
        if (isBossRoom) return bossRoomPrefab;

        float roll = Random.value;
        if (treasureRoomPrefab != null && roll < 0.1f) return treasureRoomPrefab;
        if (secretRoomPrefab != null && roll < 0.2f) return secretRoomPrefab;
        return roomPrefabs[Random.Range(0, roomPrefabs.Count)];
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return new Vector3(gridPos.x * cellSize, 0, gridPos.y * cellSize);
    }

    void ClearPreviousDungeon()
    {
        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        spawnedRoomsGrid.Clear();
        spawnedRooms.Clear();
    }

    void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }

    void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        Gizmos.color = Color.blue;
        foreach (Vector2Int pos in spawnedRoomsGrid)
        {
            Vector3 worldPos = GridToWorld(pos);
            Gizmos.DrawWireCube(worldPos, Vector3.one * roomSize);
        }

        // Стартовая комната - зеленый
        Gizmos.color = Color.green;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one * roomSize * 1.1f);
    }
}