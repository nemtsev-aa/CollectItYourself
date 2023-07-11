using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;
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
    [XmlElement("ID")]
    [SerializeField] private int id;
    /// <summary>
    /// Количество очков за выполнение
    /// </summary>
    [XmlElement("ExpValue")]
    [SerializeField] private int expValue;
    /// <summary>
    /// Модификация задания
    /// </summary>
    [XmlElement("TaskMode")]
    [SerializeField] private TaskMode mode;
    /// <summary>
    /// Тип задания
    /// </summary>
    [XmlElement("TaskType")]
    [SerializeField] private TaskType type;
    /// <summary>
    /// Вариант задания
    /// </summary>
    [XmlElement("TaskVariant")]
    [SerializeField] private int variant;
    /// <summary>
    /// Принципиальная схема
    /// </summary>
    [XmlElement("PrincipalShema")]
    [SerializeField] private List<Sprite> principalShema;
    /// <summary>
    /// Комплектация распределительной коробки
    /// </summary>
    [XmlElement("SwitchBoxsData")]
    [SerializeField] private List<SwitchBoxData> switchBoxsData;
    /// <summary>
    /// Верное подключение компанентов
    /// </summary>
    [XmlElement("Answer")]
    [SerializeField] private List<Answer> answer;


    public int ID => id;
    public int ExpValue => expValue;
    public TaskMode Mode => mode;
    public TaskType Type => type;
    public int Variant => variant;
    public List<Sprite> PrincipalShema => principalShema;
    public List<SwitchBoxData> SwitchBoxsData => switchBoxsData;
    public List<Answer> Answer => answer;

}
