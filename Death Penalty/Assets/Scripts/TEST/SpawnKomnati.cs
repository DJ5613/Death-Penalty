using UnityEngine;
using System.Collections.Generic;

public class RoomSpawner : MonoBehaviour
{
    public GameObject[] roomPrefabs;       // Обычные комнаты
    public GameObject bossRoomPrefab;      // Комната с боссом
    public int gridSize = 5;              // Размер сетки
    public float roomSize = 10f;          // Размер комнаты
    public int minRooms = 5;              // Минимум комнат
    public int maxRooms = 10;             // Максимум комнат

    private Dictionary<Vector2Int, GameObject> spawnedRooms = new Dictionary<Vector2Int, GameObject>();
    private Queue<Vector2Int> roomsToProcess = new Queue<Vector2Int>();
    private int totalRoomsToSpawn;
    private int roomsSpawned;

    void Start()
    {
        totalRoomsToSpawn = Random.Range(minRooms, maxRooms + 1);
        Debug.Log($"Будет создано {totalRoomsToSpawn} комнат, последняя — с боссом.");

        // Спавним стартовую комнату
        Vector2Int startPos = new Vector2Int(gridSize / 2, gridSize / 2);
        SpawnRoom(startPos, Quaternion.identity, false);

        // Генерация остальных комнат
        while (roomsToProcess.Count > 0 && roomsSpawned < totalRoomsToSpawn)
        {
            Vector2Int currentPos = roomsToProcess.Dequeue();
            SpawnAdjacentRooms(currentPos);
        }

        // Заменяем последнюю комнату на босса
        if (roomsSpawned > 0)
        {
            ReplaceLastRoomWithBoss();
        }
    }

    void SpawnAdjacentRooms(Vector2Int pos)
    {
        List<Vector2Int> directions = new List<Vector2Int> { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left };
        Shuffle(directions);

        foreach (var dir in directions)
        {
            if (roomsSpawned >= totalRoomsToSpawn) break;

            Vector2Int newPos = pos + dir;
            if (CanSpawnRoom(newPos))
            {
                TrySpawnRoom(pos, dir, newPos);
            }
        }
    }

    void TrySpawnRoom(Vector2Int prevPos, Vector2Int dir, Vector2Int newPos)
    {
        GameObject prevRoom = spawnedRooms[prevPos];
        Transform prevExit = FindExit(prevRoom, dir);

        if (prevExit == null)
        {
            Debug.Log($"У комнаты в {prevPos} нет выхода в направлении {dir}");
            return;
        }

        // Пытаемся найти подходящую комнату (включая повороты)
        GameObject roomPrefab = null;
        Quaternion roomRotation = Quaternion.identity;

        // Перемешиваем комнаты для большей вариативности
        List<GameObject> shuffledRooms = new List<GameObject>(roomPrefabs);
        Shuffle(shuffledRooms);

        foreach (var prefab in shuffledRooms)
        {
            // Проверяем все возможные повороты (0°, 90°, 180°, 270°)
            for (int rot = 0; rot < 4; rot++)
            {
                Quaternion testRotation = Quaternion.Euler(0, rot * 90, 0);
                Vector3 rotatedEntranceDir = testRotation * Vector3.forward;

                // Преобразуем направление входа в глобальные координаты
                Vector2Int entranceDir = new Vector2Int(
                    Mathf.RoundToInt(rotatedEntranceDir.x),
                    Mathf.RoundToInt(rotatedEntranceDir.z)
                );

                // Если вход комнаты совпадает с нужным направлением (-dir)
                if (entranceDir == -dir)
                {
                    roomPrefab = prefab;
                    roomRotation = testRotation;
                    break;
                }
            }

            if (roomPrefab != null) break;
        }

        if (roomPrefab == null)
        {
            Debug.LogWarning($"Не удалось найти комнату с входом в направлении {-dir}");
            return;
        }

        // Поворачиваем комнату так, чтобы её вход смотрел на выход предыдущей
        Quaternion finalRotation = Quaternion.LookRotation(prevExit.forward, Vector3.up) * roomRotation;
        SpawnRoom(newPos, finalRotation, false);
    }

    Transform FindExit(GameObject room, Vector2Int dir)
    {
        foreach (Transform child in room.transform)
        {
            if (child.CompareTag("Exit"))
            {
                Vector3 exitForward = child.forward;
                Vector2Int exitDir = new Vector2Int(
                    Mathf.RoundToInt(exitForward.x),
                    Mathf.RoundToInt(exitForward.z)
                );

                if (exitDir == dir)
                {
                    return child;
                }
            }
        }
        return null;
    }

    bool CanSpawnRoom(Vector2Int pos)
    {
        if (pos.x < 0 || pos.x >= gridSize || pos.y < 0 || pos.y >= gridSize)
            return false;
        return !spawnedRooms.ContainsKey(pos);
    }

    void SpawnRoom(Vector2Int pos, Quaternion rotation, bool isBossRoom)
    {
        if (roomsSpawned >= totalRoomsToSpawn) return;

        Vector3 spawnPos = new Vector3(pos.x, 0, pos.y) * roomSize;
        GameObject roomPrefab = isBossRoom ? bossRoomPrefab : roomPrefabs[Random.Range(0, roomPrefabs.Length)];
        GameObject newRoom = Instantiate(roomPrefab, spawnPos, rotation);

        spawnedRooms.Add(pos, newRoom);
        roomsToProcess.Enqueue(pos);
        roomsSpawned++;
    }

    void ReplaceLastRoomWithBoss()
    {
        Vector2Int lastPos = new Vector2Int();
        foreach (var pos in spawnedRooms.Keys)
        {
            lastPos = pos;
        }

        Destroy(spawnedRooms[lastPos]);
        spawnedRooms.Remove(lastPos);

        // Находим направление от предыдущей комнаты к последней
        Vector2Int dirToBoss = lastPos - FindAdjacentRoomPos(lastPos);
        Quaternion bossRotation = Quaternion.LookRotation(new Vector3(dirToBoss.x, 0, dirToBoss.y), Vector3.up);

        SpawnRoom(lastPos, bossRotation, true);
        Debug.Log("Комната с боссом создана!");
    }

    Vector2Int FindAdjacentRoomPos(Vector2Int pos)
    {
        foreach (var dir in new Vector2Int[] { Vector2Int.up, Vector2Int.right, Vector2Int.down, Vector2Int.left })
        {
            Vector2Int adjacentPos = pos + dir;
            if (spawnedRooms.ContainsKey(adjacentPos))
            {
                return adjacentPos;
            }
        }
        return pos; // fallback
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            T temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }
}