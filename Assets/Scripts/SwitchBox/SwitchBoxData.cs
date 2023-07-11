using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Данные о подключении
/// </summary>
[System.Serializable]
public struct ConnectionData {
    public CompanentType CompanentType;
    public string CompanentName;
    public ContactType ContactType;
}
/// <summary>
/// Данные для размещения компанента в распределительной каробке
/// </summary>
[System.Serializable]
public struct CompanentData {
    public int SlotNumber;
    public string Name;
    public TaskMode TaskMode;
    public CompanentType CompanentType;
    public VersionExecution VersionExecution;
}
/// <summary>
/// Данные отражающие содержание распределительной коробки
/// </summary>
[CreateAssetMenu(fileName = nameof(SwitchBoxData), menuName = "Tasks/ScriptableObjects/" + nameof(SwitchBoxData), order = 2)]
public class SwitchBoxData : ScriptableObject {
    [Tooltip("Номер части")]
    public int PartNumber;
    [Tooltip("Список компанентов")]
    public List<CompanentData> Companents = new List<CompanentData>();
}
