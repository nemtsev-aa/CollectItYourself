using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ConnectionData {
    public Companent Companent;
    public Contact Contact;
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
    public TaskType TaskType;

    public List<CompanentData> Companents = new List<CompanentData>();
}