using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Answer), menuName = "Tasks/ScriptableObjects/" + nameof(Answer), order = 3)]
public class Answer : ScriptableObject {
    public int WagoClipNumber;
    public List<AnswerData> AnswerDataList = new (); 
}
