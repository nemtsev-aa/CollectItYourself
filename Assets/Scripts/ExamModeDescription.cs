using UnityEngine;

public enum ExamModeType {
    KnownSchemes,
    UnknownSchemes,
}

[CreateAssetMenu(fileName = nameof(ExamModeDescription), menuName = "ExamMode/" + nameof(ExamModeDescription))]
[System.Serializable]
public class ExamModeDescription : Description {
    public ExamModeType CurrentType;
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
