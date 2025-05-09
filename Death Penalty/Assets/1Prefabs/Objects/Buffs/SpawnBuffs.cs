using UnityEngine;
using System.Collections.Generic;

public class SpawnBuffs : MonoBehaviour
{
    [SerializeField] private List<GameObject> buffPrefabs; // Список префабов баффов
    [SerializeField] private Transform spawnPoint; // Точка спавна

    private void Start()
    {

    }

    public void SpawnRandomBuff()
    {
        // Выбираем случайный бафф из списка
        int randomIndex = Random.Range(0, buffPrefabs.Count);
        GameObject buffToSpawn = buffPrefabs[randomIndex];

        // Создаем бафф в точке спавна
        Instantiate(buffToSpawn, spawnPoint.position, spawnPoint.rotation);
    }

    // Метод для ручного вызова спавна (например, из другого скрипта или события)
    public void SpawnBuffNow()
    {
        SpawnRandomBuff();
    }
}