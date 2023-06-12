using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct ContactData {
    public CompanentType CompanentType;
    public string CompanentName;
    public ContactType ContactType;
}

[CreateAssetMenu(fileName = nameof(AnswerData), menuName = nameof(AnswerData))]
public class AnswerData : ScriptableObject {
    public List<ContactData> Connections = new();
}

