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
    Error�orrection,
    SaveResults,
    OutputResult
}
/// <summary>
/// ��������� ������� � ��������� ��������� ���������� � ������ "����������"
/// </summary>
public class TrainingModeController : IService, IDisposable {
    private EventBus _eventBus;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        
        _eventBus.Subscribe<TaskListCreatedSignal>(OnTaskListCreated);  // ������ ������� �������
        _eventBus.Subscribe<TaskSelectSignal>(TaskSelect);              // ������� �������
        _eventBus.Subscribe<TaskFinishedSignal>(TaskFinished);          // ������� ���������
    }

    /// <summary>
    /// ������� ������ �������
    /// </summary>
    /// <param name="signal"></param>
    public void OnTaskListCreated(TaskListCreatedSignal signal) {
        SelectTrainingTaskDialog dialog = DialogManager.ShowDialog<SelectTrainingTaskDialog>();
    }

    /// <summary>
    /// ������� �������
    /// </summary>
    /// <param name="signal"></param>
    private void TaskSelect(TaskSelectSignal signal) {
        CorrectSwitchingDialog correctSwitchingDialog = DialogManager.ShowDialog<CorrectSwitchingDialog>();
    }


    /// <summary>
    /// ������� ���������
    /// </summary>
    /// <param name="signal"></param>
    private void TaskFinished(TaskFinishedSignal signal) {
        if(signal.GeneralSwitchingResult.CheckResult) {
            // ���������� ������ � ������
            CorrectSwitchingDialog correctSwitchingDialog = DialogManager.ShowDialog<CorrectSwitchingDialog>();
            correctSwitchingDialog.Init(signal.GeneralSwitchingResult);
        }
    }

    /// <summary>
    /// ����� "����������" �������������
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
