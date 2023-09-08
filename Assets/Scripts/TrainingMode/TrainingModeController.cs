using CustomEventBus;
using CustomEventBus.Signals;
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
    ErrorСorrection,
    SaveResults,
    OutputResult
}

/// <summary>
/// Принимает решения о изменении состояния приложения в модуле "Тренировка"
/// </summary>
public class TrainingModeController : IService, IDisposable {
    public TrainingModeStatus CurrentStatus => _currentStatus;

    private TrainingModeStatus _currentStatus;
    private TrainingModeMenuDialog _trainingModeMenuDialog;
    private SelectTrainingTaskDialog _selectTrainingTaskDialog;
    private TrainingSwitchingDialog _trainingSwitchingDialog;
    private TrainingDemonstrationDialog _demonstrationDialog;
    private CorrectSwitchingResultDialog _correctSwitchingDialog;
    private IncorrectSwitchingResultDialog _incorrectSwitchingDialog;
    
    private SwitchBoxesManager _switchBoxesManager;
    private TimeManager _timeManager;
    private EventBus _eventBus;
    private TaskController _taskController;
    private TaskData _currentTaskData;

    public void Init(EventBus eventBus, TaskController taskController, SwitchBoxesManager switchBoxesManager, TimeManager timeManager) {
        _eventBus = eventBus;
        _taskController = taskController;
        _switchBoxesManager = switchBoxesManager;
        _timeManager = timeManager;

        _eventBus.Subscribe<TaskListCreatedSignal>(TaskListReceived);            // Список заданий получен
        _eventBus.Subscribe<TaskSelectSignal>(TaskSelect);                       // Задание выбрано
        _eventBus.Subscribe<TaskCheckingStartSignal>(TaskChecking);              // Запуск проверки
        _eventBus.Subscribe<TaskCheckingFinishedSignal>(PreparingDemonstration); // Завершение проверки, подготовка демонстрации
        _eventBus.Subscribe<AnswerDemonstrationSignal>(AnswerDemonstration);     // Запуск демонстрации результата сборки
        _eventBus.Subscribe<TaskFinishedSignal>(TaskFinished);                   // Задание завершено
        _eventBus.Subscribe<PauseStateSignal>(TaskPause);                        // Пауза в сборке
        _eventBus.Subscribe<TaskResumeSignal>(TaskResumeSwitching);              // Продолжение сборки
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
            case TrainingModeStatus.ErrorСorrection:
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
    /// Получение списка заданий из выбранного источника
    /// </summary>
    public void CreateTaskListFromSource() {
        _taskController.CreateTaskListFromSource();
        _currentStatus = TrainingModeStatus.LoadTaskData;
    }

    /// <summary>
    /// Получен список заданий
    /// </summary>
    /// <param name="signal"></param>
    public void TaskListReceived(TaskListCreatedSignal signal) {
        if (_trainingSwitchingDialog != null) ResetSwitchingZone();
             
        _selectTrainingTaskDialog = DialogManager.ShowDialog<SelectTrainingTaskDialog>();
        _selectTrainingTaskDialog.Init(_eventBus);

        _taskController.CreateTaskMap(_selectTrainingTaskDialog);
        _taskController.CreateConnects(_selectTrainingTaskDialog.TaskConnectorsManager);

        _currentStatus = TrainingModeStatus.TaskListReceived;
    }

    /// <summary>
    /// Задание выбрано
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
    /// Запуск задания, который может быть отменён
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
    /// Переход к окну "Коммутация"
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

        _timeManager.SetTimeView(_trainingSwitchingDialog.PrincipalSchemaView.TimeView);

        Pointer pointer = ServiceLocator.Current.Get<Pointer>();
        pointer.Init(_eventBus);
        pointer.SetStatus(true);

        WagoCreator wagoCreator = ServiceLocator.Current.Get<WagoCreator>();
        wagoCreator.Init(_switchBoxesManager, pointer);
        
        Management management = ServiceLocator.Current.Get<Management>();
        management.Init();
        ServiceLocator.Current.Get<WireCreator>().Init(management, _switchBoxesManager, pointer);

        _trainingSwitchingDialog.Init(_switchBoxesManager, _timeManager, wagoCreator, _eventBus);
        _switchBoxesManager.CreateSwitchBoxs();


        _timeManager.SetStatus(true);

        _currentStatus = TrainingModeStatus.PreparingWorkspace;
    }
    
    /// <summary>
    /// Запуск проверки
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
            Debug.LogError("TrainingModeController: Ошибка в процедуре проверки сборки!");
        }
    }

    /// <summary>
    /// Завершение проверки, подготовка демонстрации
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
    /// Запуск демонстрации результата сборки
    /// </summary>
    public void AnswerDemonstration(AnswerDemonstrationSignal signal) {

    }

    /// <summary>
    /// Задание завершено
    /// </summary>
    /// <param name="signal"></param>
    private void TaskFinished(TaskFinishedSignal signal) {
        _demonstrationDialog.Hide();
        if (signal.GeneralSwitchingResult.CheckStatus) {
            // Показываем окно о верной сборке
            _correctSwitchingDialog = DialogManager.ShowDialog<CorrectSwitchingResultDialog>();
            _correctSwitchingDialog.Init(signal.GeneralSwitchingResult);
        } else {
            // Показываем окно о неверной сборке
            _incorrectSwitchingDialog = DialogManager.ShowDialog<IncorrectSwitchingResultDialog>();
            _incorrectSwitchingDialog.Init(signal.GeneralSwitchingResult);
        }
    }

    /// <summary>
    /// Режим "Тренировка" преостановлен
    /// </summary>
    public void TaskStop() {
        // Останавливаем секундомер
        // Останавливаем указатель
        // Скрываем РК
        _eventBus.Invoke(new TrainingModeStopSignal());
    }

    /// <summary>
    /// Перезагрузка зоны сборки
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

        _timeManager.ResetTimer();
        _switchBoxesManager.Reset();
    }

    /// <summary>
    /// Пауза в сбоке
    /// </summary>
    /// <param name="signal"></param>
    private void TaskPause(PauseStateSignal signal) {
        // Останавливаем секундомер
        // Останавливаем указатель
        // Скрываем РК
        _eventBus.Invoke(new TrainingModeStopSignal());

        DialogManager.ShowDialog<PauseDialog>().Init();                            // Отображаем окно "Пауза"
    }

    /// <summary>
    /// Возврат к процессу коммутации
    /// </summary>
    /// <param name="signal"></param>
    private void TaskResumeSwitching(TaskResumeSignal signal) {
        if (_demonstrationDialog != null) _demonstrationDialog.Hide();
        _eventBus.Invoke(new TrainingModeStartSignal()); // Включаем секундомер, Включаем указатель, Показываем РК
        _trainingSwitchingDialog.gameObject.SetActive(true);
    }

    public void Dispose() {
        _eventBus.Unsubscribe<TaskListCreatedSignal>(TaskListReceived);             // Список заданий получен
        _eventBus.Unsubscribe<TaskSelectSignal>(TaskSelect);                        // Задание выбрано
        _eventBus.Unsubscribe<TaskCheckingStartSignal>(TaskChecking);               // Запуск проверки
        _eventBus.Unsubscribe<AnswerDemonstrationSignal>(AnswerDemonstration);      // Демонстрация результата
        _eventBus.Unsubscribe<TaskFinishedSignal>(TaskFinished);                    // Задание завершено
        _eventBus.Unsubscribe<PauseStateSignal>(TaskPause);                          // Пауза в сборке
        _eventBus.Unsubscribe<TaskResumeSignal>(TaskResumeSwitching);               // Продолжение сборки
    }
}
