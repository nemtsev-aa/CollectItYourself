using System.Collections.Generic;
using UnityEngine;
public enum TaskStatus {
    Lock,
    Unlock,
    Complite
}

public enum TaskMode {
    Training,
    Exam
}
public enum TaskType {
    Part1,
    Part2,
    Part3,
    Part4,
    Full
}

[CreateAssetMenu(fileName = nameof(TaskData), menuName = "Tasks/ScriptableObjects/" + nameof(TaskData))]
[System.Serializable]
public class TaskData : ScriptableObject {
    /// <summary>
    /// Уникальный ID задания
    /// </summary>
    public string ID => _id;
    /// <summary>
    /// Статус задания
    /// </summary>
    public TaskStatus TaskStatus => _status;
    /// <summary>
    /// Количество очков за выполнение
    /// </summary>
    public int ExpValue => _expValue;
    /// <summary>
    /// Количество монет за выполнение
    /// </summary>
    public int CoinsCount => _coinsCount;
    /// <summary>
    /// Модификация задания
    /// </summary>
    public TaskMode Mode => _mode;
    /// <summary>
    /// Тип задания
    /// </summary>
    public TaskType Type => _type;
    /// <summary>
    /// Вариант задания
    /// </summary>
    public int Variant => _variant;
    /// <summary>
    /// Принципиальная схема
    /// </summary>
    public Sprite WiringShema => _wiringShema;
    /// <summary>
    /// Принципиальная схема
    /// </summary>
    public List<Sprite> PrincipalShemas => _principalShema;
    /// <summary>
    /// Комплектация распределительной коробки
    /// </summary>
    public List<SwitchBoxData> SwitchBoxsData => _switchBoxsData;
    /// <summary>
    /// Верное подключение компанентов
    /// </summary>
    public List<Answer> Answers => _answers;
    /// <summary>
    /// Верное подключение компанентов
    /// </summary>
    public List<TaskData> NextTaskData => _nextTaskData;
    /// <summary>
    /// Статистика задания
    /// </summary>
    public TaskStatistics TaskStatistics => _taskStatistics;

    
    [SerializeField] private string _id;
    [SerializeField] private TaskStatus _status;
    [SerializeField] private int _expValue;
    [SerializeField] private int _coinsCount;
    [SerializeField] private TaskMode _mode;
    [SerializeField] private TaskType _type;
    [SerializeField] private int _variant;
    [SerializeField] private Sprite _wiringShema;
    [SerializeField] private List<Sprite> _principalShema;
    [SerializeField] private List<SwitchBoxData> _switchBoxsData;
    [SerializeField] private List<Answer> _answers;
    [SerializeField] private List<TaskData> _nextTaskData;
    [SerializeField] private TaskStatistics _taskStatistics;

    /// <summary>
    /// Общее количество подключений в ответе
    /// </summary>
    /// <returns></returns>
    public int GetConnectionsCount() {
        int connectionsCount = 0;
        foreach (var iAnswer in Answers) {
            foreach (var iAnswerData in iAnswer.AnswerDataList) {
                connectionsCount += iAnswerData.Connections.Count;
            }
        }
        return connectionsCount;
    }
    /// <summary>
    /// Количество подключений в отдельной РК
    /// </summary>
    /// <returns></returns>
    public int GetConnectionsCountInSwitchBox(int index = 0) {
        int connectionsCount = 0;
        foreach (var iAnswerData in Answers[index].AnswerDataList) {
            connectionsCount += iAnswerData.Connections.Count;
        }
        return connectionsCount;
    }

    public void SetStatus(TaskStatus status) {
        _status = status;
    }

    public void SetTaskStatistics(TaskStatistics value) {
        _taskStatistics = value;
    }
}
