using System;
using System.Collections.Generic;
using UI;
using UI.Dialogs;
using UnityEngine;

public class DialogManager {
    private const string PrefabsFilePath = "Dialogs/";

    // При создании новых окон добавлять их сюда
    private static readonly Dictionary<Type, string> PrefabsDictionary = new Dictionary<Type, string>() {
            {typeof(MainMenuDialog),"MenuDialog/MainMenuDialog"},
            {typeof(PauseDialog),"MenuDialog/PauseDialog"},
            {typeof(SettingsDialog),"MenuDialog/SettingsDialog"},

            {typeof(LearningModeMenuDialog),"MenuDialog/LearningMode/LearningModeMenuDialog"},
            
            {typeof(TrainingModeMenuDialog),"MenuDialog/TrainingMode/TrainingModeMenuDialog"},
            {typeof(SelectTrainingTaskDialog),"MenuDialog/TrainingMode/SelectTrainingTaskDialog"},

            {typeof(ExamModeMenuDialog),"MenuDialog/ExamMode/ExamModeMenuDialog"},
            {typeof(SelectExamTaskDialog),"MenuDialog/ExamMode/SelectExamTaskDialog"},

            {typeof(CountdownDialog), "CountdownDialog"},
            {typeof(CorrectSwitchingResultDialog),"CorrectSwitchingResultDialog"},
            {typeof(IncorrectSwitchingResultDialog),"IncorrectSwitchingResultDialog"},
            {typeof(LoadingDialog),"LoadingDialog"},
            {typeof(TaskDescriptionDialog),"TaskDescriptionDialog"},
            {typeof(TaskUnlockedDialog),"TaskUnlockedDialog"},
            {typeof(TheoreticalPartDialog),"TheoreticalPartDialog"},
            {typeof(TrainingDemonstrationDialog), "TrainingDemonstrationDialog"},
            {typeof(TrainingSwitchingDialog),"TrainingSwitchingDialog"},
            
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
    /// Ссылка на канвас на сцене, чтобы к нему подцеплялись наши окна
    /// </summary>
    public static Transform GuiHolder {
        get { return ServiceLocator.Current.Get<GUIHolder>().transform; }
    }
}

