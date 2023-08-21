using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// ����� ��� ����� ���������� �� ������� �������
/// </summary>
[CreateAssetMenu(fileName = nameof(TaskStatistics), menuName = "Tasks/ScriptableObjects/" + nameof(TaskStatistics))]
[System.Serializable]
public class TaskStatistics : ScriptableObject {
    /// <summary>
    /// ���������� ID �������
    /// </summary>
    public string ID { get { return _id; } private set { _id = value; } }
    /// <summary>
    /// ������ �������
    /// </summary>
    public IEnumerable<AttemptInfo> Attempts { get { return _attempts; } private set { _attempts = (List<AttemptInfo>)value; } }
    /// <summary>
    /// ���������� ������ ("����� �������" / "����� ������")
    /// </summary>
    public string CorrectSwitchingCount { get { return _correctSwitchingCount; } private set { _correctSwitchingCount = value; } }
    /// <summary>
    /// ������ ����� ������
    /// </summary>
    public string BestBuildingTime { get { return _bestBuildingTime; } private set { _bestBuildingTime = value; } }

    [SerializeField] private string _id;
    [SerializeField] private List<AttemptInfo> _attempts;
    [SerializeField] private string _correctSwitchingCount;
    [SerializeField] private string _bestBuildingTime;

    // ����� �������� ���������� ������
    public static TaskStatistics CreateInstance(string id, List<AttemptInfo> attempts, string correctSwitchingCount, string bestBuildingTime) {

        TaskStatistics instance = CreateInstance<TaskStatistics>();
        instance.ID = id;
        instance.Attempts = attempts;
        instance.CorrectSwitchingCount = correctSwitchingCount;
        instance.BestBuildingTime = bestBuildingTime;


#if UNITY_EDITOR
        // ������� ���� ��� ���������� ScriptableObject
        string path = $"Assets/Resources/TaskStatistics/Statistic_{id}.asset";

        if (AssetDatabase.IsValidFolder(path)) {        // ���������, ���������� �� ���� �� ���������� ����
            AssetDatabase.DeleteAsset(path);            // ���� ���� ��� ����������, ������� ���
        }

        AssetDatabase.CreateAsset(instance, path);      // ������� ������ �� ���������� ����
        AssetDatabase.Refresh();                        // ��������� �������� ���� ������ ��������
#endif
        return instance;
    }
}
