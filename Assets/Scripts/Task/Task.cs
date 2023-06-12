using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum TaskType {
    Training,
    Exam
}

[CreateAssetMenu(fileName = nameof(Task), menuName = nameof(Task))]
public class Task : ScriptableObject {
    [Tooltip("�������� �������")]
    public string Name;
    [Tooltip("��� �������")]
    public TaskType TaskType;
    [Tooltip("������ �������")]
    public List<TaskData> TaskData;
}
