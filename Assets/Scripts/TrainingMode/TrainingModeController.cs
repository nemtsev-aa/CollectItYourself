using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using UI.Dialogs;
using UnityEngine;

public enum TrainingModeStatus {
    LoadTaskData,
    TaskListReceived,
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
    public TrainingModeStatus CurrentStatus => _currentStatus;

    private TrainingModeStatus _currentStatus;
    private SelectTrainingTaskDialog _selectTrainingTaskDialog;
    private TrainingSwitchingDialog _trainingSwitchingDialog;
    private TaskConnectorsManager _taskConnectorsManager;
    private EventBus _eventBus;
    private TaskController _taskController;
    private TaskData _currentTaskData;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _taskController = ServiceLocator.Current.Get<TaskController>();

        _eventBus.Subscribe<TaskListCreatedSignal>(TaskListReceived);            // ������ ������� �������
        _eventBus.Subscribe<TaskSelectSignal>(TaskSelect);                       // ������� �������
        _eventBus.Subscribe<TaskCheckingStartSignal>(TaskChecking);              // ������ ��������
        _eventBus.Subscribe<AnswerDemonstrationSignal>(AnswerDemonstration);     // ������ ��������
        _eventBus.Subscribe<TaskFinishedSignal>(TaskFinished);                   // ������� ���������
        _eventBus.Subscribe<TaskPauseSignal>(TaskPause);                         // ����� � ������
        _eventBus.Subscribe<TaskResumeSignal>(TaskResumeSwitching);              // ����������� ������
    }
    
    public void SetTrainingModeStatus(TrainingModeStatus status) {
        switch (status) {
            case TrainingModeStatus.LoadTaskData:
                CreateTaskListFromSource();
                break;
            case TrainingModeStatus.TaskListReceived:

                break;
            case TrainingModeStatus.LoadProgressValue:
                break;
            case TrainingModeStatus.ShowProgress:
                break;
            case TrainingModeStatus.SelectionTask:
                break;
            case TrainingModeStatus.PreparingWorkspace:
                break;
            case TrainingModeStatus.Switching:
                break;
            case TrainingModeStatus.CheckResult:
                break;
            case TrainingModeStatus.Demonstration:
                break;
            case TrainingModeStatus.Error�orrection:
                break;
            case TrainingModeStatus.SaveResults:
                break;
            case TrainingModeStatus.OutputResult:
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ��������� ������ ������� �� ���������� ���������
    /// </summary>
    public void CreateTaskListFromSource() {
        _taskController.CreateTaskListFromSource();
    }

    /// <summary>
    /// ������� ������ �������
    /// </summary>
    /// <param name="signal"></param>
    public void TaskListReceived(TaskListCreatedSignal signal) {
        _currentStatus = TrainingModeStatus.TaskListReceived;

        _selectTrainingTaskDialog = DialogManager.ShowDialog<SelectTrainingTaskDialog>();
        _taskConnectorsManager = _selectTrainingTaskDialog.TaskConnectorsManager;
        _selectTrainingTaskDialog.TrainingProgressView.Init();
        _selectTrainingTaskDialog.GoldCountView.Init();

        _taskController.CreateTaskMap(_selectTrainingTaskDialog, _taskConnectorsManager);
        _taskController.CreateConnects();
    }

    /// <summary>
    /// ������� �������
    /// </summary>
    /// <param name="signal"></param>
    private void TaskSelect(TaskSelectSignal signal) {
        _currentTaskData = signal.TaskData;
        //_taskController.
        CountdownDialog countdownDialog = DialogManager.ShowDialog<CountdownDialog>();
        countdownDialog.Init();
        countdownDialog.OnCountdownFinish += TaskStarted;
    }
   
    /// <summary>
    /// ������� �����������, �� ����� ����������� ��� ������
    /// </summary>
    /// <param name="status"></param>
    private void TaskStarted(bool status) {
        if (status) {
            ShowTask();
        } else {
            _currentTaskData = null;
        }
    }

    /// <summary>
    /// ������� � ���� "����������"
    /// </summary>
    /// <param name="selectionTask"></param>
    public void ShowTask() {
        _selectTrainingTaskDialog.Hide();
        Debug.Log("ShowTask" + _currentTaskData.ID.ToString());
        _eventBus.Invoke(new TaskStartedSignal(_currentTaskData));

        _trainingSwitchingDialog = DialogManager.ShowDialog<TrainingSwitchingDialog>();
        SwitchBoxManager switchBoxManager = ServiceLocator.Current.Get<SwitchBoxManager>();
        switchBoxManager.Init(_currentTaskData, _eventBus);
        switchBoxManager.SetSwitchBoxsSelectorView(_trainingSwitchingDialog.SwitchBoxsSelectorView);
        _trainingSwitchingDialog.SwitchBoxsSelectorView.Init(switchBoxManager);

        Stopwatch stopwatch = ServiceLocator.Current.Get<Stopwatch>();
        stopwatch.Init(_trainingSwitchingDialog.PrincipalSchemaView.TimeView);

        Pointer pointer = ServiceLocator.Current.Get<Pointer>();
        pointer.Init();
        pointer.SetStatus(true);

        WagoCreator wagoCreator = ServiceLocator.Current.Get<WagoCreator>();
        wagoCreator.Init(switchBoxManager, pointer);
        
        Management management = ServiceLocator.Current.Get<Management>();
        management.Init();
        ServiceLocator.Current.Get<WireCreator>().Init(management);

        _trainingSwitchingDialog.Init(switchBoxManager, stopwatch, wagoCreator, _eventBus);
        
        switchBoxManager.CreateSwitchBoxs();
        stopwatch.SetStatus(true);
    }
    
    /// <summary>
    /// ������ ��������
    /// </summary>
    /// <param name="signal"></param>
    public void TaskChecking(TaskCheckingStartSignal signal) {
        TaskStop();

        GeneralSwitchingResult newResult = ServiceLocator.Current.Get<SwitchBoxManager>().CheckSwichBoxes();
        if (newResult != null) {
            if (_currentTaskData.TaskStatistics.AddAttempt(newResult)) {
                Debug.Log("��������� �������� � ���������� �������!");
            } else {
                Debug.LogError("������ ��� ���������� ���������� � ���������� �������!");
            }

            if (newResult.CheckResult) { // ������ ������
                
            } else {

            }

        } else {
            Debug.LogError("TrainingModeController: ������ � ��������� �������� ������!");
        }
    }

    /// <summary>
    /// ������ ������������ ���������� ������
    /// </summary>
    public void AnswerDemonstration(AnswerDemonstrationSignal signal) {

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
    public void TaskStop() {
        ServiceLocator.Current.Get<Stopwatch>().SetStatus(false);           // ������������� ����������
        ServiceLocator.Current.Get<Pointer>().SetStatus(false);             // ������������� ���������
        ServiceLocator.Current.Get<SwitchBoxManager>().HideSwitchBoxs();    // �������� ��
        
        //_eventBus.Invoke(new TrainingModeStopSignal());
    }

    public void ClearTask(TaskData hideTask) {
        //SwitchBoxManager.RemoveSwichBoxFromList(SwitchBoxManager.ActiveSwichBox);
    }

    /// <summary>
    /// ����� � �����
    /// </summary>
    /// <param name="signal"></param>
    private void TaskPause(TaskPauseSignal signal) {
        ServiceLocator.Current.Get<Stopwatch>().SetStatus(false);           // ������������� ����������
        ServiceLocator.Current.Get<Pointer>().SetStatus(false);             // ������������� ���������
        ServiceLocator.Current.Get<SwitchBoxManager>().HideSwitchBoxs();    // �������� ��
        PauseDialog dialog = DialogManager.ShowDialog<PauseDialog>();                            // ���������� ���� "�����"
        dialog.Init();
    }

    /// <summary>
    /// ������� � �������� ����������
    /// </summary>
    /// <param name="signal"></param>
    private void TaskResumeSwitching(TaskResumeSignal signal) {
        ServiceLocator.Current.Get<Stopwatch>().SetStatus(true);            // �������� ����������
        ServiceLocator.Current.Get<Pointer>().SetStatus(true);              // �������� ���������
        ServiceLocator.Current.Get<SwitchBoxManager>().ShowSwitchBoxs();    // ���������� ��
    }

    public void Dispose() {
        _eventBus.Unsubscribe<TaskListCreatedSignal>(TaskListReceived);  // ������ ������� �������
        _eventBus.Unsubscribe<TaskSelectSignal>(TaskSelect);              // ������� �������
        _eventBus.Unsubscribe<TaskFinishedSignal>(TaskFinished);          // ������� ���������
        _eventBus.Unsubscribe<TaskPauseSignal>(TaskPause);                // ����� � ������
        _eventBus.Unsubscribe<TaskResumeSignal>(TaskResumeSwitching);     // ����������� ������
    }
}
