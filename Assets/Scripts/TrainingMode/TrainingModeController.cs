using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UI.Dialogs;
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
        _eventBus.Subscribe<TaskPauseSignal>(TaskPause);                 // ����� � ������
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
        ShowTask(signal.TaskData);
    }

    /// <summary>
    /// ������� � ���� "����������"
    /// </summary>
    /// <param name="selectionTask"></param>
    public void ShowTask(TaskData selectionTask) {
        SwitchBoxManager switchBoxManager = ServiceLocator.Current.Get<SwitchBoxManager>();
        switchBoxManager.Init(selectionTask);
        switchBoxManager.CreateSwitchBoxs();

        Management management = ServiceLocator.Current.Get<Management>();
        management.Init();
        ServiceLocator.Current.Get<WireCreator>().Init(management);
       
        Pointer pointer = ServiceLocator.Current.Get<Pointer>();
        pointer.Init();

        Stopwatch stopwatch = ServiceLocator.Current.Get<Stopwatch>();
        stopwatch.Init();

        TrainingSwitchingDialog newSwitching = DialogManager.ShowDialog<TrainingSwitchingDialog>();
        newSwitching.Init(switchBoxManager, stopwatch);
        ServiceLocator.Current.Get<WagoCreator>().Init(switchBoxManager, pointer);

        
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

    

    public void ClearTask(TaskData hideTask) {
        //SwitchBoxManager.RemoveSwichBoxFromList(SwitchBoxManager.ActiveSwichBox);
    }

    private void TaskResumeSwitching() {
        _eventBus.Invoke(new TaskResumeSignal());
    }

    /// <summary>
    /// ����� � �����
    /// </summary>
    /// <param name="signal"></param>
    private void TaskPause(TaskPauseSignal signal) {
        ServiceLocator.Current.Get<Stopwatch>().SetStatus(false);           // ������������� ����������
        ServiceLocator.Current.Get<Pointer>().SetStatus(false);             // ������������� ���������
        ServiceLocator.Current.Get<SwitchBoxManager>().HideSwitchBoxs();    // �������� ��
        DialogManager.ShowDialog<PauseDialog>();                            // ���������� ���� "�����"
    }

    public void Dispose() {
        _eventBus.Unsubscribe<TaskSelectSignal>(TaskSelect);
    }


}
