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
    private TrainingDemonstrationDialog _demonstrationDialog;
    private CorrectSwitchingResultDialog _correctSwitchingDialog;
    private IncorrectSwitchingResultDialog _incorrectSwitchingDialog;
    
    
    private SwitchBoxesManager _switchBoxesManager;
    private Stopwatch _stopwatch;
    private EventBus _eventBus;
    private TaskController _taskController;
    private TaskData _currentTaskData;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _taskController = ServiceLocator.Current.Get<TaskController>();
        _switchBoxesManager = ServiceLocator.Current.Get<SwitchBoxesManager>();
        _stopwatch = ServiceLocator.Current.Get<Stopwatch>();

        _eventBus.Subscribe<TaskListCreatedSignal>(TaskListReceived);            // ������ ������� �������
        _eventBus.Subscribe<TaskSelectSignal>(TaskSelect);                       // ������� �������
        _eventBus.Subscribe<TaskCheckingStartSignal>(TaskChecking);              // ������ ��������
        _eventBus.Subscribe<TaskCheckingFinishedSignal>(PreparingDemonstration); // ���������� ��������, ���������� ������������
        _eventBus.Subscribe<AnswerDemonstrationSignal>(AnswerDemonstration);     // ������ ������������ ���������� ������
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
        _currentStatus = TrainingModeStatus.LoadTaskData;
    }

    /// <summary>
    /// ������� ������ �������
    /// </summary>
    /// <param name="signal"></param>
    public void TaskListReceived(TaskListCreatedSignal signal) {
        if (_trainingSwitchingDialog != null) ResetSwitchingZone();
             
        _selectTrainingTaskDialog = DialogManager.ShowDialog<SelectTrainingTaskDialog>();
        _selectTrainingTaskDialog.Init();

        _taskController.CreateTaskMap(_selectTrainingTaskDialog);
        _taskController.CreateConnects(_selectTrainingTaskDialog.TaskConnectorsManager);

        _currentStatus = TrainingModeStatus.TaskListReceived;
    }

    /// <summary>
    /// ������� �������
    /// </summary>
    /// <param name="signal"></param>
    private void TaskSelect(TaskSelectSignal signal) {
        _currentTaskData = signal.TaskData;
        CountdownDialog countdownDialog = DialogManager.ShowDialog<CountdownDialog>();
        countdownDialog.Init();
        countdownDialog.OnCountdownFinish += TaskStarted;

        _currentStatus = TrainingModeStatus.SelectionTask;
    }

    /// <summary>
    /// ������ �������, ������� ����� ���� ������
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
        ResetSwitchingZone();

        Debug.Log("ShowTask" + _currentTaskData.ID.ToString());
        _eventBus.Invoke(new TaskStartedSignal(_currentTaskData));

        _trainingSwitchingDialog = DialogManager.ShowDialog<TrainingSwitchingDialog>();
        
        _switchBoxesManager.Init(_currentTaskData, _eventBus);
        _switchBoxesManager.SetSwitchBoxsSelectorView(_trainingSwitchingDialog.SwitchBoxsSelectorView);
        _switchBoxesManager.SetSwitchBoxProgress(_trainingSwitchingDialog.SwitchingProgress);
        _trainingSwitchingDialog.SwitchBoxsSelectorView.Init(_switchBoxesManager);

        _stopwatch.SetTimeView(_trainingSwitchingDialog.PrincipalSchemaView.TimeView);

        Pointer pointer = ServiceLocator.Current.Get<Pointer>();
        pointer.Init(_eventBus);
        pointer.SetStatus(true);

        WagoCreator wagoCreator = ServiceLocator.Current.Get<WagoCreator>();
        wagoCreator.Init(_switchBoxesManager, pointer);
        
        Management management = ServiceLocator.Current.Get<Management>();
        management.Init();
        ServiceLocator.Current.Get<WireCreator>().Init(management, _switchBoxesManager, pointer);

        _trainingSwitchingDialog.Init(_switchBoxesManager, _stopwatch, wagoCreator, _eventBus);
        _switchBoxesManager.CreateSwitchBoxs();


        _stopwatch.SetStatus(true);

        _currentStatus = TrainingModeStatus.PreparingWorkspace;
    }
    
    /// <summary>
    /// ������ ��������
    /// </summary>
    /// <param name="signal"></param>
    public void TaskChecking(TaskCheckingStartSignal signal) {
        _currentStatus = TrainingModeStatus.CheckResult;

        TaskStop();

        _trainingSwitchingDialog.gameObject.SetActive(false);
        GeneralSwitchingResult newResult = _switchBoxesManager.CheckSwichBoxes();
        if (newResult != null) {
            _eventBus.Invoke(new TaskCheckingFinishedSignal(newResult));
        } else {
            Debug.LogError("TrainingModeController: ������ � ��������� �������� ������!");
        }
    }

    /// <summary>
    /// ���������� ��������, ���������� ������������
    /// </summary>
    /// <param name="obj"></param>
    private void PreparingDemonstration(TaskCheckingFinishedSignal signal) {
        //if (signal.GeneralSwitchingResult.CheckStatus == true) {

        //} else {

        //}

        _demonstrationDialog = DialogManager.ShowDialog<TrainingDemonstrationDialog>();
        _demonstrationDialog.Init(signal.GeneralSwitchingResult, _eventBus);
        _currentStatus = TrainingModeStatus.Demonstration;

        //_switchBoxesManager.ShowSwitchBoxs();
        //_switchBoxesManager.ShowCurrent(true);
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
        _demonstrationDialog.Hide();
        if (signal.GeneralSwitchingResult.CheckStatus) {
            // ���������� ���� � ������ ������
            _correctSwitchingDialog = DialogManager.ShowDialog<CorrectSwitchingResultDialog>();
            _correctSwitchingDialog.Init(signal.GeneralSwitchingResult);
        } else {
            // ���������� ���� � �������� ������
            _incorrectSwitchingDialog = DialogManager.ShowDialog<IncorrectSwitchingResultDialog>();
            _incorrectSwitchingDialog.Init(signal.GeneralSwitchingResult);
        }
    }

    /// <summary>
    /// ����� "����������" �������������
    /// </summary>
    public void TaskStop() {
        // ������������� ����������
        // ������������� ���������
        // �������� ��
        _eventBus.Invoke(new TrainingModeStopSignal());
    }

    /// <summary>
    /// ������������ ���� ������
    /// </summary>
    /// <param name="signal"></param>
    private void ResetSwitchingZone() {
        if (_trainingSwitchingDialog != null) {
            _trainingSwitchingDialog.Hide();
        }
        if (_selectTrainingTaskDialog != null) {
            _taskController.Reset();
            _selectTrainingTaskDialog.Hide();
        }

        _stopwatch.ResetTimer();
        _switchBoxesManager.Reset();
    }

    /// <summary>
    /// ����� � �����
    /// </summary>
    /// <param name="signal"></param>
    private void TaskPause(TaskPauseSignal signal) {
        // ������������� ����������
        // ������������� ���������
        // �������� ��
        _eventBus.Invoke(new TrainingModeStopSignal());

        DialogManager.ShowDialog<PauseDialog>().Init();                            // ���������� ���� "�����"
    }

    /// <summary>
    /// ������� � �������� ����������
    /// </summary>
    /// <param name="signal"></param>
    private void TaskResumeSwitching(TaskResumeSignal signal) {
        if (_demonstrationDialog != null) _demonstrationDialog.Hide();
        _eventBus.Invoke(new TrainingModeStartSignal()); // �������� ����������, �������� ���������, ���������� ��
        _trainingSwitchingDialog.gameObject.SetActive(true);
    }

    public void Dispose() {
        _eventBus.Unsubscribe<TaskListCreatedSignal>(TaskListReceived);             // ������ ������� �������
        _eventBus.Unsubscribe<TaskSelectSignal>(TaskSelect);                        // ������� �������
        _eventBus.Unsubscribe<TaskCheckingStartSignal>(TaskChecking);               // ������ ��������
        _eventBus.Unsubscribe<AnswerDemonstrationSignal>(AnswerDemonstration);      // ������������ ����������
        _eventBus.Unsubscribe<TaskFinishedSignal>(TaskFinished);                    // ������� ���������
        _eventBus.Unsubscribe<TaskPauseSignal>(TaskPause);                          // ����� � ������
        _eventBus.Unsubscribe<TaskResumeSignal>(TaskResumeSwitching);               // ����������� ������
    }
}
