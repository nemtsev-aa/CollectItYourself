using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using IDisposable = CustomEventBus.IDisposable;

public enum DataSource {
    ScriptableObject,
    Json,
    Xml
}

/// <summary>
/// Загрузчик сервисов для модуля "Тренировка"
/// </summary>
public class ServiceLocatorLoader_TrainingMode : MonoBehaviour {
    [SerializeField] private DataSource _currentDataSource;
    [SerializeField] private GUIHolder _guiHolder;
    [SerializeField] private TrainingModeController _trainingModeController; // Контроль состояния приложения
    [SerializeField] private TaskController _taskController;                 // Менеджер заданий
    [SerializeField] private TrainingModeProgressManager _trainingModeProgressManager;   // Прогресс в модуле "Тренировка"
    [SerializeField] private Management _management;                         // Менеджер игрового процесса
    [SerializeField] private SwitchBoxManager _switchBoxManager;             // Менеджер Распределительных коробок
    [SerializeField] private GoldController _goldController;                 // Менеджер золота
    [SerializeField] private WagoCreator _wagoCreator;                       // Генератор Wago-зажимов
    [SerializeField] private WireCreator _wireCreator;                       // Генератор проводов
    [SerializeField] private Pointer _pointer;                               // Указатель
    [SerializeField] private Stopwatch _stopwatch;                           // Секундомер
    [SerializeField] private ProgressView _progressView;                     // Виджет прогресса
    [SerializeField] private GoldCountView _goldView;                        // Виджет золота
    [SerializeField] private ScriptableObjectTaskLoader _scriptableObjectTaskLoader;
    
    private ITaskLoader _taskLoader;                                         // Загрузчик заданий из различных источников
    private List<IDisposable> _disposables = new List<IDisposable>();        // Интерфейс для отписки от сигнальной шины
    private ConfigDataLoader _configDataLoader;
    private EventBus _eventBus;

    private void Awake() {
        _eventBus = new EventBus();
        _trainingModeController = new TrainingModeController();
        _trainingModeProgressManager = new TrainingModeProgressManager();
        _goldController = new GoldController();

        switch (_currentDataSource) {
            case DataSource.ScriptableObject:
                _taskLoader = _scriptableObjectTaskLoader;
                break;
            case DataSource.Json:
                _taskLoader = new JsonTasksLoader("TasksConfig.json");
                break;
            case DataSource.Xml:
                //_taskLoader = new XmlTasksLoader("TasksConfig.json");
                break;
            default:
                break;
        }

        //Debug.Log("Awake complite");
        RegisterServices();
        Init();
        AddDisposables();
    }

    private void RegisterServices() {
        ServiceLocator.Initialize();

        ServiceLocator.Current.Register(_eventBus);
        ServiceLocator.Current.Register(_guiHolder);
        ServiceLocator.Current.Register(_trainingModeController);
        ServiceLocator.Current.Register(_trainingModeProgressManager);
        ServiceLocator.Current.Register(_taskController);
        ServiceLocator.Current.Register(_taskLoader);
        ServiceLocator.Current.Register(_management);
        ServiceLocator.Current.Register(_switchBoxManager);
        ServiceLocator.Current.Register(_goldController);
        ServiceLocator.Current.Register(_progressView);
        ServiceLocator.Current.Register(_goldView);
        ServiceLocator.Current.Register(_wireCreator);
        ServiceLocator.Current.Register(_wagoCreator);
        ServiceLocator.Current.Register(_pointer);
        ServiceLocator.Current.Register(_stopwatch);

        //Debug.Log("RegisterServices complite");
    }

    private void Init() {
        _goldController.Init();
        _trainingModeController.Init();
        _taskController.Init();
        _progressView.Init();
        _trainingModeProgressManager.Init(_progressView);

        var loaders = new List<ILoader> {
            _taskLoader
        };

        _configDataLoader = new ConfigDataLoader();
        _configDataLoader.Init(loaders);

        //Debug.Log("InitServices complite");
        _trainingModeController.SetTrainingModeStatus(TrainingModeStatus.LoadTaskData);
    }

    private void AddDisposables() {
        _disposables.Add(_taskController);
        _disposables.Add(_progressView);

    }

    private void OnDestroy() {
        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }
    }
}
