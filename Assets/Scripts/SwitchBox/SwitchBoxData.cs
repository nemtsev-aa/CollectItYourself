using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ � �����������
/// </summary>
[System.Serializable]
public struct ConnectionData {
    public CompanentType CompanentType;
    public string CompanentName;
    public ContactType ContactType;
}
/// <summary>
/// ������ ��� ���������� ���������� � ����������������� �������
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
/// ������ ���������� ���������� ����������������� �������
/// </summary>
[CreateAssetMenu(fileName = nameof(SwitchBoxData), menuName = "Tasks/ScriptableObjects/" + nameof(SwitchBoxData), order = 2)]
public class SwitchBoxData : ScriptableObject {
    [Tooltip("����� �����")]
    public int PartNumber;
    [Tooltip("������ �����������")]
    public List<CompanentData> Companents = new List<CompanentData>();
}
