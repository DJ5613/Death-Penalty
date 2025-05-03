using System.Collections.Generic;
using UnityEngine;

public class BuffSystem : MonoBehaviour
{
    // Список доступных баффов
    private List<BuffType> availableBuffs = new List<BuffType>();

    // Инициализация при старте игры
    void Start()
    {
        InitializeBuffs();
    }

    // Заполняем список всеми возможными баффами
    private void InitializeBuffs()
    {
        availableBuffs.Clear();
        foreach (BuffType buff in System.Enum.GetValues(typeof(BuffType)))
        {
            availableBuffs.Add(buff);
        }
    }

    // Метод для получения случайного баффа
    public void GetRandomBuff()
    {
        if (availableBuffs.Count == 0)
        {
            Debug.Log("Все баффы уже использованы!");
            return;
        }

        // Выбираем случайный индекс
        int randomIndex = Random.Range(0, availableBuffs.Count);
        BuffType selectedBuff = availableBuffs[randomIndex];

        // Удаляем выбранный бафф из списка доступных
        availableBuffs.RemoveAt(randomIndex);

        // Применяем бафф
        ApplyBuff(selectedBuff);
    }

    // Применение конкретного баффа
    private void ApplyBuff(BuffType buff)
    {
        switch (buff)
        {
            case BuffType.SpeedBoost:
                // Логика для увеличения скорости
                break;
            case BuffType.JumpBoost:
                // Логика для увеличения прыжка
                break;
                // Добавьте обработку для остальных баффов
        }
        Debug.Log("Applied buff: " + buff);
    }

    // Перечисление возможных баффов
    public enum BuffType
    {
        SpeedBoost,
        JumpBoost,
        DamageBoost,
        DefenseBoost,
        HealthRegen,
        CriticalChance,
        Invisibility
    }
}