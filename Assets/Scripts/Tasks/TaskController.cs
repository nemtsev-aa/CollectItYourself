using System.Collections.Generic;
using System.Linq;
using CustomEventBus;
using CustomEventBus.Signals;
using Cysharp.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Отвечает за логику заданий:
/// Переключает текущее задание на следующее
/// Уведомляет остальные системы что задание изменилось
/// Уведомляет что задание пройдено
/// </summary>
public class TaskController : MonoBehaviour, IService, IDisposable {
    public string CurrentTaskId => _currentTaskId;
    public TaskData CurrentTaskData => _currentTaskData;
    [SerializeField] private TasksConfig _tasksConfig;

    [SerializeField] private List<TaskVariantCard> _taskVariantCards = new List<TaskVariantCard>();
    [SerializeField] TaskConnectorsManager _taskConnectorsManager;
    //[SerializeField] private TaskVariantCard _startCard;
    //[SerializeField] private List<TaskVariantCard> _nextCards;

    private ITaskLoader _taskLoader;
    private string _currentTaskId;
    private TaskData _currentTaskData;
    private EventBus _eventBus;

    public TaskData FindTask(string id) {
        foreach (var iTask in _tasksConfig.Tasks) {
            if (iTask.ID == id) {
                _currentTaskData = iTask;
                return iTask;
            }
        }
        _currentTaskData = null;
        return null;
    }

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<TaskTimePassedSignal>(TaskTimePassed);
        _eventBus.Subscribe<TaskNextSignal>(NextTask);
        _eventBus.Subscribe<RestartTaskSignal>(RestartTask);

        _taskLoader = ServiceLocator.Current.Get<ITaskLoader>();

        OnInit();
    }

    private async void OnInit() {
        await UniTask.WaitUntil(_taskLoader.IsLoaded);
        _currentTaskData = _taskLoader.GetTasks().FirstOrDefault(x => x.ID == _currentTaskId);
        if (_currentTaskData == null) {
            Debug.LogErrorFormat("Can't find task with id {0}", _currentTaskId);
            return;
        }
        _eventBus.Invoke(new TaskSelectSignal(_currentTaskData));
    }

    private void NextTask(TaskNextSignal signal) {
        //_currentTaskId++;
        //SelectTask(_currentTaskId);
    }

    private void RestartTask(RestartTaskSignal signal) {
        _eventBus.Invoke(new TaskSelectSignal(_currentTaskData));
    }

    private void SelectTask(string taskId) {
        _currentTaskId = taskId;
        _currentTaskData = _taskLoader.GetTasks().FirstOrDefault(x => x.ID == _currentTaskId);
        _eventBus.Invoke(new TaskSelectSignal(_currentTaskData));
    }

    private void TaskTimePassed(TaskTimePassedSignal signal) {
        /// Убедиться в том, что РК не собрана полностью и принять решение о дальнейших действиях
        if (true) {

        }
        /// Сборка на время - проработать основную механику и систему вознаграждения и штрафов

        //PlayerPrefs.SetInt(StringConstants.CURRENT_TASK, (_currentTaskId + 1));
        _eventBus.Invoke(new TaskFinishedSignal(signal.GeneralSwitchingResult));
    }

    public void Dispose() {
        _eventBus.Unsubscribe<TaskNextSignal>(NextTask);
        _eventBus.Unsubscribe<TaskTimePassedSignal>(TaskTimePassed);
    }

    [ContextMenu("CreateConnect")]
    public void CreateConnects() {
        if (_taskVariantCards.Count < 1) Debug.LogError("TaskController: карточки с заданиями не добавлены в список"); 
        foreach (TaskVariantCard iTaskCard in _taskVariantCards) {
            
        }
        //_taskConnectorsManager.CreateConnect(_startCard, _nextCards);
    }
}