using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ����������� ��������� �������
/// </summary>
[CreateAssetMenu(fileName = nameof(TasksConfig), menuName = "Tasks/ScriptableObjects/" + nameof(TasksConfig))]
public class TasksConfig : ScriptableObject {
    [SerializeField] private List<TaskData> _tasks;
    public IEnumerable<TaskData> Tasks => _tasks;
}
