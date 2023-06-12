using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(Answer), menuName = nameof(Answer))]
public class Answer : ScriptableObject
{
    public int WagoClipNumber;
    public List<AnswerData> AnswerDataList = new (); 
}
