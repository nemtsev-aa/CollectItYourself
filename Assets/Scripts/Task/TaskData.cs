using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TaskData), menuName = nameof(TaskData))]
public class TaskData : ScriptableObject {
    [Tooltip("Принципиальная схема")]
    public Sprite PrincipalShemas;
    [Tooltip("Комплектация распределительной коробки")]
    public SwitchBoxData SwitchBoxsData;
    [Tooltip("Верное подключение компанентов")]
    public Answer Answer;
}
