using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = nameof(AnswerData), menuName = nameof(AnswerData))]
public class AnswerData : ScriptableObject {
    public List<ConnectionData> Connections = new();
}

