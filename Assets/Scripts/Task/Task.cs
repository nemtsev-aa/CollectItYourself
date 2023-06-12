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
    [Tooltip("Название задания")]
    public string Name;
    [Tooltip("Тип задания")]
    public TaskType TaskType;
    [Tooltip("Данные задания")]
    public List<TaskData> TaskData;
}
