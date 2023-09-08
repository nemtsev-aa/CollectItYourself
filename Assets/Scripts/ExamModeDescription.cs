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
    /// Количество опыта для завершения
    /// </summary>
    [field: SerializeField] public int ExpAmountToComplete { get; set; }
    /// <summary>
    /// Количество опыта для разблокирования
    /// </summary>
    [field: SerializeField] public int ExpAmountToUnlock { get; set; }
    /// <summary>
    /// Количество полученного опыта
    /// </summary>
    [field: SerializeField] public int CurrentExpAmount;
}
