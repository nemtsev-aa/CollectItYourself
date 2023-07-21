using UnityEngine;
public enum VersionExecution {
    Right,
    Left,
    Up,
    Down
}
/// <summary>
/// Данные о префабах доступных компанентов
/// </summary>
[System.Serializable]
public class CompanentPrefabData {
    [Tooltip("Модификация задания")]
    [SerializeField] private TaskMode _taskMode;
    [Tooltip("Тип компанента")]
    [SerializeField] private CompanentType _companentType;
    [Tooltip("Версия исполнения")]
    [SerializeField] private VersionExecution _versionExecution;
    [Tooltip("Префаб")]
    [SerializeField] private Companent _prefab;

    public TaskMode TaskMode =>_taskMode;
    public CompanentType CompanentType => _companentType;
    public VersionExecution VersionExecution => _versionExecution;
    public Companent Prefab => _prefab;
}
