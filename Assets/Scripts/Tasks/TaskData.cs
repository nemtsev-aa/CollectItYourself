using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
public enum TaskStatus {
    Lock,
    Unlock,
    Complete
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
    [XmlElement("ID")]
    [SerializeField] private string _id;
    /// <summary>
    /// ���������� ID �������
    /// </summary>
    [XmlElement("TaskStatus")]
    [SerializeField] private TaskStatus _status;
    /// <summary>
    /// ���������� ����� �� ����������
    /// </summary>
    [XmlElement("ExpValue")]
    [SerializeField] private int _expValue;
    /// <summary>
    /// ���������� ����� �� ����������
    /// </summary>
    [XmlElement("CoinsCount")]
    [SerializeField] private int _coinsCount;
    /// <summary>
    /// ����������� �������
    /// </summary>
    [XmlElement("TaskMode")]
    [SerializeField] private TaskMode _mode;
    /// <summary>
    /// ��� �������
    /// </summary>
    [XmlElement("TaskType")]
    [SerializeField] private TaskType _type;
    /// <summary>
    /// ������� �������
    /// </summary>
    [XmlElement("TaskVariant")]
    [SerializeField] private int _variant;
    /// <summary>
    /// �������������� �����
    /// </summary>
    [XmlElement("WiringShema")]
    [SerializeField] private Sprite _wiringShema;
    /// <summary>
    /// �������������� �����
    /// </summary>
    [XmlElement("PrincipalShema")]
    [SerializeField] private List<Sprite> _principalShema;
    /// <summary>
    /// ������������ ����������������� �������
    /// </summary>
    [XmlElement("SwitchBoxsData")]
    [SerializeField] private List<SwitchBoxData> _switchBoxsData;
    /// <summary>
    /// ������ ����������� �����������
    /// </summary>
    [XmlElement("Answer")]
    [SerializeField] private List<Answer> _answer;
    /// <summary>
    /// ������ ����������� �����������
    /// </summary>
    [XmlElement("NextTask")]
    [SerializeField] private List<TaskData> _nextTaskData;
    /// <summary>
    /// ���������� �������
    /// </summary>
    [XmlElement("TaskStatistics")]
    [SerializeField] private TaskStatistics _taskStatistics;

    public string ID => _id;
    public TaskStatus TaskStatus => _status;
    public int ExpValue => _expValue;
    public int CoinsCount => _coinsCount;
    public TaskMode Mode => _mode;
    public TaskType Type => _type;
    public int Variant => _variant;
    public Sprite WiringShema => _wiringShema;
    public List<Sprite> PrincipalShemas => _principalShema;
    public List<SwitchBoxData> SwitchBoxsData => _switchBoxsData;
    public List<Answer> Answers => _answer;
    public List<TaskData> NextTaskData => _nextTaskData;
    public TaskStatistics TaskStatistics => _taskStatistics;

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

    public void SetStatus(TaskStatus status) {
        _status = status;
    }
}
