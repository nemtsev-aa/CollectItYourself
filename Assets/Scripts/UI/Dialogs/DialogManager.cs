using System;
using System.Collections.Generic;
using UI;
using UI.Dialogs;
using UnityEngine;

public class DialogManager {
    private const string PrefabsFilePath = "Dialogs/";

    // ��� �������� ����� ���� ��������� �� ����
    private static readonly Dictionary<Type, string> PrefabsDictionary = new Dictionary<Type, string>() {
            {typeof(MainMenuDialog),"MenuDialog/MainMenuDialog"},
            {typeof(SelectLearningChapterDialog),"MenuDialog/SelectLearningChapterDialog"},
            {typeof(SelectTrainingTaskDialog),"MenuDialog/SelectTrainingTaskDialog"},
            //{typeof(SelectExamTaskDialog),"MenuDialog/SelectExamTaskDialog"},
            {typeof(PauseDialog),"MenuDialog/PauseDialog"},
            {typeof(SettingsDialog),"MenuDialog/SettingsDialog"},

            {typeof(CountdownDialog), "CountdownDialog"},
            {typeof(TrainingSwitchingDialog),"TrainingSwitchingDialog"},
            {typeof(CorrectSwitchingResultDialog),"CorrectSwitchingResultDialog"},
            {typeof(IncorrectSwitchingResultDialog),"IncorrectSwitchingResultDialog"},
            {typeof(LoadingDialog),"LoadingDialog"},
            {typeof(TaskDescriptionDialog),"TaskDescriptionDialog"},
            {typeof(TaskUnlockedDialog),"TaskUnlockedDialog"},
           
            //{typeof(MenuDialog),"MenuDialogs/MenuDialog"},
            //{typeof(ScoreTableDialog),"MenuDialogs/ScoreTableDialog"},
            //{typeof(CustomizeShipDialog),"MenuDialogs/CustomizeShipDialog"},
    };

    public static T ShowDialog<T>() where T : Dialog {
        var go = GetPrefabByType<T>();
        if (go == null) {
            Debug.LogError("Show window - object not found");
            return null;
        }

        return GameObject.Instantiate(go, GuiHolder);
    }

    private static T GetPrefabByType<T>() where T : Dialog {
        var prefabName = PrefabsDictionary[typeof(T)];
        if (string.IsNullOrEmpty(prefabName)) {
            Debug.LogError("cant find prefab type of " + typeof(T) + "Do you added it in PrefabsDictionary?");
        }

        var path = PrefabsFilePath + PrefabsDictionary[typeof(T)];
        var dialog = Resources.Load<T>(path);
        if (dialog == null) {
            Debug.LogError("Cant find prefab at path " + path);
        }

        return dialog;
    }

    /// <summary>
    /// ������ �� ������ �� �����, ����� � ���� ������������ ���� ����
    /// </summary>
    public static Transform GuiHolder {
        get { return ServiceLocator.Current.Get<GUIHolder>().transform; }
    }
}
