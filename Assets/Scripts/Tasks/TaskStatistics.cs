using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Класс для сбора статистики по каждому заданию
/// </summary>
[CreateAssetMenu(fileName = nameof(TaskStatistics), menuName = "Tasks/ScriptableObjects/" + nameof(TaskStatistics))]
[System.Serializable]
public class TaskStatistics : ScriptableObject {
    /// <summary>
    /// Уникальный ID задания
    /// </summary>
    public string ID { get { return _id; } private set { _id = value; } }
    /// <summary>
    /// Список попыток
    /// </summary>
    public IEnumerable<AttemptInfo> Attempts { get { return _attempts; } private set { _attempts = (List<AttemptInfo>)value; } }
    /// <summary>
    /// Статистика сборки ("верно собрано" / "всего сборок")
    /// </summary>
    public string CorrectSwitchingCount { get { return _correctSwitchingCount; } private set { _correctSwitchingCount = value; } }
    /// <summary>
    /// Лучшее время сборки
    /// </summary>
    public string BestBuildingTime { get { return _bestBuildingTime; } private set { _bestBuildingTime = value; } }

    [SerializeField] private string _id;
    [SerializeField] private List<AttemptInfo> _attempts;
    [SerializeField] private string _correctSwitchingCount;
    [SerializeField] private string _bestBuildingTime;

    // Метод создания экземпляра класса
    public static TaskStatistics CreateInstance(string id, List<AttemptInfo> attempts, string correctSwitchingCount, string bestBuildingTime) {

        TaskStatistics instance = CreateInstance<TaskStatistics>();
        instance.ID = id;
        instance.Attempts = attempts;
        instance.CorrectSwitchingCount = correctSwitchingCount;
        instance.BestBuildingTime = bestBuildingTime;


#if UNITY_EDITOR
        // Создаем путь для сохранения ScriptableObject
        string path = $"Assets/Resources/TaskStatistics/Statistic_{id}.asset";

        if (AssetDatabase.IsValidFolder(path)) {        // Проверяем, существует ли файл по указанному пути
            AssetDatabase.DeleteAsset(path);            // Если файл уже существует, удаляем его
        }

        AssetDatabase.CreateAsset(instance, path);      // Создаем ресурс по указанному пути
        AssetDatabase.Refresh();                        // Обновляем активную базу данных ресурсов
#endif
        return instance;
    }
}
