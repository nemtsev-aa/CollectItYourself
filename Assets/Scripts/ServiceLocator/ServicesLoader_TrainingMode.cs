using CustomEventBus;
using System.Collections.Generic;
using UI;
using UI.Dialogs;
using UnityEngine;
using IDisposable = CustomEventBus.IDisposable;

public enum DataSource {
    ScriptableObject,
    Json
}

/// <summary>
/// Загрузчик сервисов для модуля "Тренировка"
/// </summary>
public class ServicesLoader_TrainingMode : ServicesLoader {
    [SerializeField] private DataSource _currentDataSource;
    [SerializeField] private GUIHolder _guiHolder;                                          // Контейнер для всплывающих окон
    [SerializeField] private TrainingModeController _trainingModeController;                // Менеджер состояния приложения в режиме "Тренировка"
    [SerializeField] private TaskController _taskController;                                // Менеджер заданий
    [SerializeField] private TrainingModeProgressManager _trainingModeProgressManager;      // Прогресс в модуле "Тренировка"
    [SerializeField] private Management _management;                                        // Менеджер игрового процесса
    [SerializeField] private SwitchBoxesManager _switchBoxesManager;                        // Менеджер Распределительных коробок
    [SerializeField] private WagoCreator _wagoCreator;                                      // Генератор Wago-зажимов
    [SerializeField] private WireCreator _wireCreator;                                      // Генератор проводов
    [SerializeField] private Pointer _pointer;                                              // Указатель
    [SerializeField] private TimeManager _timeManager;                                      // Секундомер
    [SerializeField] private TrainingProgressView _progressView;                            // Виджет прогресса

    [SerializeField] private ScriptableObjectTaskLoader _scriptableObjectTaskLoader;

    private GoldController _goldController;                                                 // Менеджер золота
    private GoldCountView _goldView;                                                        // Виджет золота
    private AttemptsLog _attemptsLog;                                                       // Журнал попыток
    private ITaskLoader _taskLoader;                                                        // Загрузчик заданий из различных источников
    private ConfigDataLoader _configDataLoader;
    private TrainingModeMenuDialog _trainingModeMenuDialog;
    private TrainingModeType _currentTrainingModeType;

    private void Start() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _savesManager = ServiceLocator.Current.Get<SavesManager>();
        _goldController = ServiceLocator.Current.Get<GoldController>();

        ServiceLocator.Current.RegisterWithReplacement(_guiHolder);

        ShowTrainingModeMenu();
    }

    private void ShowTrainingModeMenu() {
        _trainingModeMenuDialog = DialogManager.ShowDialog<TrainingModeMenuDialog>();
        _trainingModeMenuDialog.Init(this);
        _goldView = _trainingModeMenuDialog.GoldCountView;
        _goldView.Init(_eventBus, _goldController);
    }

    public void SetTrainingModeType(TrainingModeType type) {
        _trainingModeMenuDialog.Hide();
        _currentTrainingModeType = type;
        Init();
        RegisterServices();
        AddDisposables();
    }

    public override void Init() {
        switch (_currentTrainingModeType) {
            case TrainingModeType.RepeatAfterCoach:
                InitMode_RepeatAfterCoach();
                break;
            case TrainingModeType.BuildByVariants:
                InitMode_BuildByVariants();
                break;
            case TrainingModeType.TroubleFinding:
                InitMode_TroubleFinding();
                break;
            default:
                break;
        }
    }

    private void InitMode_RepeatAfterCoach() {
        Debug.Log("InitMode_RepeatAfterCoach complite");
    }

    private void InitMode_BuildByVariants() {
        //if (_currentDataSource == DataSource.ScriptableObject) _taskLoader = _scriptableObjectTaskLoader;
        //else _taskLoader = new JsonTasksLoader("TasksConfig.json");

        //_attemptsLog = new AttemptsLog();
        //_attemptsLog.Init(_savesManager, _eventBus);

        //_taskController.Init(_attemptsLog);
        //_timeManager.Init(_eventBus);

        //_trainingModeController = new TrainingModeController();
        //_trainingModeController.Init(_eventBus, _taskController, _switchBoxesManager, _timeManager);

        //_trainingModeProgressManager = new TrainingModeProgressManager();
        //_trainingModeProgressManager.Init();

        //var loaders = new List<ILoader> {
        //    _taskLoader
        //};

        //_configDataLoader = new ConfigDataLoader();
        //_configDataLoader.Init(loaders);

        //_trainingModeController.SetTrainingModeStatus(TrainingModeStatus.LoadTaskData);
        Debug.Log("InitMode_BuildByVariants complite");
    }

    private void InitMode_TroubleFinding() {
        Debug.Log("InitMode_TroubleFinding complite");
    }

    public override void RegisterServices() {
 
        switch (_currentTrainingModeType) {
            case TrainingModeType.RepeatAfterCoach:
                RegisterServices_RepeatAfterCoach();
                break;
            case TrainingModeType.BuildByVariants:
                RegisterServices_BuildByVariants();
                break;
            case TrainingModeType.TroubleFinding:
                RegisterServices_TroubleFinding();
                break;
            default:
                break;
        }

        
    }

    private void RegisterServices_RepeatAfterCoach() {
        Debug.Log("RegisterServices_RepeatAfterCoach complite");
    }

    private void RegisterServices_BuildByVariants() {
        
        //ServiceLocator.Current.Register(_trainingModeController);
        //ServiceLocator.Current.Register(_trainingModeProgressManager);
        //ServiceLocator.Current.Register(_taskController);
        //ServiceLocator.Current.Register(_taskLoader);
        //ServiceLocator.Current.Register(_management);
        //ServiceLocator.Current.Register(_switchBoxesManager);
        //ServiceLocator.Current.Register(_progressView);
        //ServiceLocator.Current.Register(_wireCreator);
        //ServiceLocator.Current.Register(_wagoCreator);
        //ServiceLocator.Current.Register(_pointer);
        //ServiceLocator.Current.Register(_timeManager);
        //ServiceLocator.Current.Register(_attemptsLog);

        Debug.Log("RegisterServices_BuildByVariants complite");
    }

    private void RegisterServices_TroubleFinding() {
        Debug.Log("RegisterServices_TroubleFinding complite");
    }

    public override void AddDisposables() {
        _disposables.Add(_taskController);
        _disposables.Add(_trainingModeProgressManager);
        _disposables.Add(_trainingModeController);
        _disposables.Add(_switchBoxesManager);
        _disposables.Add(_pointer);
        _disposables.Add(_timeManager);
        _disposables.Add(_attemptsLog);

        Debug.Log("AddDisposables complite");
    }

    public override void OnDestroy() {
        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }
    }
}
