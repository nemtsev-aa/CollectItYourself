using CustomEventBus;
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
/// ��������� �������� ��� ������ "����������"
/// </summary>
public class ServiceLocatorLoader_TrainingMode : MonoBehaviour {
    [SerializeField] private DataSource _currentDataSource;
    [SerializeField] private GUIHolder _guiHolder;
    [SerializeField] private TrainingModeController _trainingModeController;                // �������� ��������� ���������� � ������ "����������"
    [SerializeField] private TaskController _taskController;                                // �������� �������
    [SerializeField] private TrainingModeProgressManager _trainingModeProgressManager;      // �������� � ������ "����������"
    [SerializeField] private Management _management;                                        // �������� �������� ��������
    [SerializeField] private SwitchBoxesManager _switchBoxesManager;                        // �������� ����������������� �������
    [SerializeField] private GoldController _goldController;                                // �������� ������
    [SerializeField] private WagoCreator _wagoCreator;                                      // ��������� Wago-�������
    [SerializeField] private WireCreator _wireCreator;                                      // ��������� ��������
    [SerializeField] private Pointer _pointer;                                              // ���������
    [SerializeField] private Stopwatch _stopwatch;                                          // ����������
    [SerializeField] private TrainingProgressView _progressView;                            // ������ ���������
    [SerializeField] private GoldCountView _goldView;                                       // ������ ������
    [SerializeField] private SavesManager _savesManager;                                    // �������� ����������
    [SerializeField] private ScriptableObjectTaskLoader _scriptableObjectTaskLoader;

    private AttemptsLog _attemptsLog;                                                       // ������ �������
    private ITaskLoader _taskLoader;                                                        // ��������� ������� �� ��������� ����������
    private List<IDisposable> _disposables = new List<IDisposable>();                       // ��������� ��� ������� �� ���������� ����
    private ConfigDataLoader _configDataLoader;
    private EventBus _eventBus;

    private void Awake() {
        _eventBus = new EventBus();
        _trainingModeController = new TrainingModeController();
        _trainingModeProgressManager = new TrainingModeProgressManager();
        _goldController = new GoldController();
        _attemptsLog = new AttemptsLog();

        switch (_currentDataSource) {
            case DataSource.ScriptableObject:
                _taskLoader = _scriptableObjectTaskLoader;
                break;
            case DataSource.Json:
                _taskLoader = new JsonTasksLoader("TasksConfig.json");
                break;
            default:
                break;
        }

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
        ServiceLocator.Current.Register(_switchBoxesManager);
        ServiceLocator.Current.Register(_goldController);
        ServiceLocator.Current.Register(_progressView);
        ServiceLocator.Current.Register(_goldView);
        ServiceLocator.Current.Register(_wireCreator);
        ServiceLocator.Current.Register(_wagoCreator);
        ServiceLocator.Current.Register(_pointer);
        ServiceLocator.Current.Register(_stopwatch);
        ServiceLocator.Current.Register(_savesManager);
        ServiceLocator.Current.Register(_attemptsLog);

        //Debug.Log("RegisterServices complite");
    }

    private void Init() {
        _savesManager.Init();
        _attemptsLog.Init(_savesManager, _eventBus);
        _goldController.Init();
        _trainingModeController.Init();
        _taskController.Init(_attemptsLog);
        _trainingModeProgressManager.Init();
        _stopwatch.Init(_eventBus);

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
        _disposables.Add(_trainingModeProgressManager);
        _disposables.Add(_trainingModeController);
        _disposables.Add(_switchBoxesManager);
        _disposables.Add(_goldController);
        _disposables.Add(_pointer);
        _disposables.Add(_stopwatch);
        _disposables.Add(_savesManager);
        _disposables.Add(_attemptsLog);
    }

    private void OnDestroy() {
        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }
    }
}
