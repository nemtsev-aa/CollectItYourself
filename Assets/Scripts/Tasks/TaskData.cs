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
    /// ���������� ID �������
    /// </summary>
    public string ID => _id;
    /// <summary>
    /// ������ �������
    /// </summary>
    public TaskStatus TaskStatus => _status;
    /// <summary>
    /// ���������� ����� �� ����������
    /// </summary>
    public int ExpValue => _expValue;
    /// <summary>
    /// ���������� ����� �� ����������
    /// </summary>
    public int CoinsCount => _coinsCount;
    /// <summary>
    /// ����������� �������
    /// </summary>
    public TaskMode Mode => _mode;
    /// <summary>
    /// ��� �������
    /// </summary>
    public TaskType Type => _type;
    /// <summary>
    /// ������� �������
    /// </summary>
    public int Variant => _variant;
    /// <summary>
    /// �������������� �����
    /// </summary>
    public Sprite WiringShema => _wiringShema;
    /// <summary>
    /// �������������� �����
    /// </summary>
    public List<Sprite> PrincipalShemas => _principalShema;
    /// <summary>
    /// ������������ ����������������� �������
    /// </summary>
    public List<SwitchBoxData> SwitchBoxsData => _switchBoxsData;
    /// <summary>
    /// ������ ����������� �����������
    /// </summary>
    public List<Answer> Answers => _answers;
    /// <summary>
    /// ������ ����������� �����������
    /// </summary>
    public List<TaskData> NextTaskData => _nextTaskData;
    /// <summary>
    /// ���������� �������
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
    /// ����� ���������� ����������� � ������
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
    /// ���������� ����������� � ��������� ��
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
