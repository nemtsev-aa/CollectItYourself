using System.Collections.Generic;
using System.Linq;
using UnityEngine;
/// <summary>
/// Конфигурация достумных для коммутации компанентов
/// </summary>
[CreateAssetMenu(fileName = nameof(CompanentConfig), menuName = "Tasks/ScriptableObjects/" + nameof(CompanentConfig), order = 4)]
public class CompanentConfig : ScriptableObject {
    [SerializeField] private List<CompanentPrefabData> _companentPrefabsData;

    public Companent Get(CompanentType type, TaskMode taskMode, VersionExecution versionExecution) {
        var companent = _companentPrefabsData.FirstOrDefault(x =>
                x.CompanentType == type && x.TaskMode == taskMode && x.VersionExecution == versionExecution);

        if (companent == null || companent.Prefab == null) {
            Debug.LogErrorFormat("Cannot find interactable of type {0}, taskType {1}, versionExecution {3}", type, taskMode, versionExecution);
            return null;
        }

        return (Companent)companent.Prefab;
    }
}

