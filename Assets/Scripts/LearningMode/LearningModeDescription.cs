using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum LearningModeType {
    Chapter1,
    Chapter2,
    Chapter3,
    Chapter4
}

[CreateAssetMenu(fileName = nameof(LearningModeDescription), menuName = "LearningMode/" + nameof(LearningModeDescription))]
[System.Serializable]
public class LearningModeDescription : Description {
    public LearningModeType CurrentType;
    public int ProgressValue => CurrentExpAmount / ExpAmountToComplete;
    /// <summary>
    /// ���������� ����� ��� ����������
    /// </summary>
    [field: SerializeField] public int ExpAmountToComplete { get; set; }
    /// <summary>
    /// ���������� ����� ��� ���������������
    /// </summary>
    [field: SerializeField] public int ExpAmountToUnlock { get; set; }
    /// <summary>
    /// ���������� ����������� �����
    /// </summary>
    [field: SerializeField] public int CurrentExpAmount;
}
