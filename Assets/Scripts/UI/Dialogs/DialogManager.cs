using System;
using System.Collections.Generic;
using UI;
using UI.Dialogs;
using UnityEngine;

public class DialogManager {
    private const string PrefabsFilePath = "Dialogs/";

    // ��� �������� ����� ���� ��������� �� ����
    private static readonly Dictionary<Type, string> PrefabsDictionary = new Dictionary<Type, string>() {
            {typeof(MainMenuDialog),"MenuDialogs/MainMenuDialog"},
            {typeof(SelectLearningChapterDialog),"MenuDialogs/SelectLearningChapterDialog"},
            {typeof(SelectTrainingTaskDialog),"MenuDialogs/SelectTrainingTaskDialog"},
            {typeof(SelectExamTaskDialog),"MenuDialogs/SelectExamTaskDialog"},
            {typeof(PauseDialog),"MenuDialogs/PauseDialog"},

            {typeof(CorrectSwitchingDialog),"CorrectSwitchingDialog"},
            {typeof(IncorrectSwitchingDialog),"IncorrectSwitchingDialog"},
            {typeof(LoadingDialog),"LoadingDialog"},
            
            //{typeof(MenuDialog),"MenuDialogs/MenuDialog"},
            //{typeof(ScoreTableDialog),"MenuDialogs/ScoreTableDialog"},
            //{typeof(SettingsDialog),"MenuDialogs/SettingsDialog"},
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

