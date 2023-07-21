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

        CompanentPrefabData companent = _companentPrefabsData.FirstOrDefault(x =>
                x.CompanentType == type && x.TaskMode == taskMode && x.VersionExecution == versionExecution);

        //CompanentPrefabData companent = null;

        //foreach (CompanentPrefabData iCompanent in _companentPrefabsData) {
        //    if (iCompanent.CompanentType == type) {
        //        if (iCompanent.TaskMode == taskMode) {
        //            if (iCompanent.VersionExecution == versionExecution) {
        //                companent = iCompanent;
        //                Debug.Log(iCompanent.Prefab.name);
        //            }
        //        }
        //    }
        //}

        if (companent == null || companent.Prefab == null) {
            Debug.LogErrorFormat("Cannot find interactable of type {0}, taskType {1}, versionExecution {3}", type, taskMode, versionExecution);
            return null;
        } else {
            Companent findCompanent = companent.Prefab;
            return findCompanent;
        } 
    }
}

