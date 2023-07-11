using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// ������ ����������� ����������� �����
/// </summary>
[CreateAssetMenu(fileName = nameof(AnswerData), menuName = "Tasks/ScriptableObjects/" + nameof(AnswerData))]
public class AnswerData : ScriptableObject {
    public List<ConnectionData> Connections = new();
}

