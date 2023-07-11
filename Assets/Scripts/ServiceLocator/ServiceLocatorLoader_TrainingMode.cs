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
/// ��������� �������� ��� ������ "����������"
/// </summary>
public class ServiceLocatorLoader_TrainingMode : MonoBehaviour {
    [SerializeField] private DataSource _currentDataSource;
    [SerializeField] private GUIHolder _guiHolder;
    [SerializeField] private TrainingModeController _trainingModeController; // �������� ��������� ����������
    [SerializeField] private TaskController _taskController;                 // �������� �������
    [SerializeField] private TrainingModeProgressManager _trainingModeProgressManager;   // �������� � ������ "����������"
    [SerializeField] private Management _management;                         // �������� �������� ��������
    [SerializeField] private SwitchBoxManager _switchBoxManager;             // �������� ����������������� �������
    [SerializeField] private WagoCreator _wagoCreator;                       // ��������� Wago-�������
    [SerializeField] private WireCreator _wireCreator;                       // ��������� ��������
    [SerializeField] private Pointer _pointer;                               // ���������
    [SerializeField] private Stopwatch _stopwatch;                           // ����������
    [SerializeField] private ProgressView _progressView;                     // ������ ���������
    [SerializeField] private ScriptableObjectTaskLoader _scriptableObjectTaskLoader;
    
    private ITaskLoader _taskLoader;                                         // ��������� ������� �� ��������� ����������
    private List<IDisposable> _disposables = new List<IDisposable>();        // ��������� ��� ������� �� ���������� ����
    private ConfigDataLoader _configDataLoader;
    private EventBus _eventBus;

    private void Awake() {
        _eventBus = new EventBus();
        _trainingModeController = new TrainingModeController();
        _trainingModeProgressManager = new TrainingModeProgressManager();

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
        ServiceLocator.Current.Register(_wireCreator);
        ServiceLocator.Current.Register(_wagoCreator);
        ServiceLocator.Current.Register(_pointer);
    }

    private void Init() {
        _trainingModeController.Init();
        _taskController.Init();
        _progressView.Init();
        _trainingModeProgressManager.Init(_progressView);
        _management.Init();
        _switchBoxManager.Init();
        _wagoCreator.Init();
        _wireCreator.Init();
        _stopwatch.Init();
        _pointer.Init();

        var loaders = new List<ILoader> {
            _taskLoader
        };

        _configDataLoader = new ConfigDataLoader();
        _configDataLoader.Init(loaders);
    }

    private void AddDisposables() {
        _disposables.Add(_taskController);

    }

    private void OnDestroy() {
        foreach (var disposable in _disposables) {
            disposable.Dispose();
        }
    }
}
