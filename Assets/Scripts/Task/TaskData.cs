using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = nameof(TaskData), menuName = nameof(TaskData))]
public class TaskData : ScriptableObject {
    [Tooltip("�������������� �����")]
    public Sprite PrincipalShemas;
    [Tooltip("������������ ����������������� �������")]
    public SwitchBoxData SwitchBoxsData;
    [Tooltip("������ ����������� �����������")]
    public Answer Answer;
}
