using UnityEngine;
public enum VersionExecution {
    Right,
    Left,
    Up,
    Down
}
/// <summary>
/// ������ � �������� ��������� �����������
/// </summary>
[System.Serializable]
public class CompanentPrefabData {
    [Tooltip("����������� �������")]
    [SerializeField] private TaskMode _taskMode;
    [Tooltip("��� ����������")]
    [SerializeField] private CompanentType _companentType;
    [Tooltip("������ ����������")]
    [SerializeField] private VersionExecution _versionExecution;
    [Tooltip("������")]
    [SerializeField] private Companent _prefab;

    public TaskMode TaskMode =>_taskMode;
    public CompanentType CompanentType => _companentType;
    public VersionExecution VersionExecution => _versionExecution;
    public Companent Prefab => _prefab;
}
