using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TrainingModeStatus {
    LoadTaskData,
    LoadProgressValue,
    ShowProgress,
    SelectionTask,
    PreparingWorkspace,
    Switching,
    CheckResult,
    Demonstration,
    ErrorСorrection,
    SaveResults,
    OutputResult
}
/// <summary>
/// Принимает решения о изменении состояния приложения в модуле "Тренировка"
/// </summary>
public class TrainingModeController : IService, IDisposable {
    private EventBus _eventBus;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        
        _eventBus.Subscribe<TaskListCreatedSignal>(OnTaskListCreated);  // Список заданий получен
        _eventBus.Subscribe<TaskSelectSignal>(TaskSelect);              // Задание выбрано
        _eventBus.Subscribe<TaskFinishedSignal>(TaskFinished);          // Задание завершено
    }

    /// <summary>
    /// Получен список заданий
    /// </summary>
    /// <param name="signal"></param>
    public void OnTaskListCreated(TaskListCreatedSignal signal) {
        SelectTrainingTaskDialog dialog = DialogManager.ShowDialog<SelectTrainingTaskDialog>();
    }

    /// <summary>
    /// Задание выбрано
    /// </summary>
    /// <param name="signal"></param>
    private void TaskSelect(TaskSelectSignal signal) {
        CorrectSwitchingDialog correctSwitchingDialog = DialogManager.ShowDialog<CorrectSwitchingDialog>();
    }


    /// <summary>
    /// Задание завершено
    /// </summary>
    /// <param name="signal"></param>
    private void TaskFinished(TaskFinishedSignal signal) {
        if(signal.GeneralSwitchingResult.CheckResult) {
            // Показываем окошко о победе
            CorrectSwitchingDialog correctSwitchingDialog = DialogManager.ShowDialog<CorrectSwitchingDialog>();
            correctSwitchingDialog.Init(signal.GeneralSwitchingResult);
        }
    }

    /// <summary>
    /// Режим "Тренировка" преостановлен
    /// </summary>
    public void StopGame() {
        _eventBus.Invoke(new TrainingModeStopSignal());
    }

    [ContextMenu("ShowTask")]
    public void ShowTask(TaskData selectionTask) {
        //SwitchBoxData = selectionTask.TaskDataList[0].SwitchBoxsData;
        //SwitchBox newBox = SwitchBoxManager.CreateSwichBox(SwitchBoxData);
        //ActionState.ShowSwitchBox(newBox);
    }

    [ContextMenu("HideTask")]
    public void ClearTask(TaskData hideTask) {
        //SwitchBoxManager.RemoveSwichBoxFromList(SwitchBoxManager.ActiveSwichBox);
    }

    public void Dispose() {
        _eventBus.Unsubscribe<TaskSelectSignal>(TaskSelect);
    }


}
