using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConnectionData {
    public CompanentType CompanentType;
    public string CompanentName;
    public ContactType ContactType;
}

[System.Serializable]
public struct CompanentData {
    public int SlotNumber;
    public string Name;
    public Companent Companent;
}

[CreateAssetMenu(fileName = nameof(SwitchBoxData), menuName = nameof(SwitchBoxData))]
public class SwitchBoxData : ScriptableObject
{
    //public string Name;
    public int PartNumber;
    public Task Task;
    public Answer Answer;
    public List<CompanentData> Companents = new List<CompanentData>();
}
