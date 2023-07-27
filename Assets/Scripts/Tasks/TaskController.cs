using System.Collections.Generic;
using System.Linq;
using CustomEventBus;
using CustomEventBus.Signals;
using Cysharp.Threading.Tasks;
using UI.Dialogs;
using UnityEngine;

/// <summary>
/// Отвечает за логику заданий:
/// Создаёт карточки заданий из config-файла с заданиями
/// Визуализирует связи между заданиями
/// Переключает текущее задание на следующее
/// Уведомляет остальные системы что задание изменилось
/// Уведомляет что задание пройдено
/// </summary>
public class TaskController : MonoBehaviour, IService, IDisposable {
    public string CurrentTaskId => _currentTaskId;
    public TaskData CurrentTaskData => _currentTaskData;
    public TasksConfig TasksConfig => _tasksConfig;

    [SerializeField] private TasksConfig _tasksConfig;

    [SerializeField] private SelectTrainingTaskDialog _selectTrainingTaskDialog;                    // Окно выбора заданий
    [SerializeField] private TaskConnectorsManager _taskConnectorsManager;                          // Менеджер связей между заданиями
    [SerializeField] private TaskVariantCard _taskVariantCardPrefab;                                // Префаб карточки с заданием
    [SerializeField] private List<TaskVariantCard> _taskVariantCards = new List<TaskVariantCard>(); // Список карточек с заданиями

    private ITaskLoader _taskLoader;
    private string _currentTaskId;
    private TaskData _currentTaskData;
    private EventBus _eventBus;
   
    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<TaskStartedSignal>(TaskSelect);              // Задание выбрано
        _eventBus.Subscribe<TaskTimePassedSignal>(TaskTimePassed);
        _eventBus.Subscribe<TaskFinishedSignal>(NextTaskUnlocked);
        _eventBus.Subscribe<TaskNextSignal>(SetNextTask);
        _eventBus.Subscribe<RestartTaskSignal>(RestartTask);
    }

    public void CreateTaskListFromSource() {
        ///////////////////////////////////////////////////////////////////////////////////
        /// Место для добавления процедуры загрузки заданий из файлов xml, json и т.д. ////
        ///////////////////////////////////////////////////////////////////////////////////

        if (_tasksConfig.Tasks.Count() > 0) {
            _eventBus.Invoke(new TaskListCreatedSignal());
            //_eventBus.Invoke(new TaskListCreatedSignal(_tasksConfig.Tasks));
        }

        //_taskLoader = ServiceLocator.Current.Get<ITaskLoader>();
        //OnInit();
    }

    //private async void OnInit() {
    //    await UniTask.WaitUntil(_taskLoader.IsLoaded);
    //    _currentTaskData = _taskLoader.GetTasks().FirstOrDefault(x => x.ID == _currentTaskId);
    //    if (_currentTaskData == null) {
    //        Debug.LogErrorFormat("Can't find task with id {0}", _currentTaskId);
    //        return;
    //    }
    //    _eventBus.Invoke(new TaskSelectSignal(_currentTaskData));
    //}

    #region CreateTaskMap
    /// <summary>
    /// Создание карты заданий
    /// </summary>
    public void CreateTaskMap(SelectTrainingTaskDialog selectTrainingTaskDialog, TaskConnectorsManager taskConnectorsManager) {
        _selectTrainingTaskDialog = selectTrainingTaskDialog;
        _taskConnectorsManager = taskConnectorsManager;
        if (_tasksConfig != null && _tasksConfig.Tasks.Count() > 0) {
            int taskNumber = 1;
            foreach (TaskData iTaskData in _tasksConfig.Tasks) {
                RectTransform parent = new RectTransform();
                switch (iTaskData.Type) {
                        case TaskType.Full:
                        parent = _selectTrainingTaskDialog.TaskGroupParent.ElementAt(0);
                        break;
                    case TaskType.Part1:
                        parent = _selectTrainingTaskDialog.TaskGroupParent.ElementAt(1);
                        break;
                    case TaskType.Part2:
                        parent = _selectTrainingTaskDialog.TaskGroupParent.ElementAt(2);
                        break;
                    case TaskType.Part3:
                        parent = _selectTrainingTaskDialog.TaskGroupParent.ElementAt(3);
                        break;
                    case TaskType.Part4:
                        parent = _selectTrainingTaskDialog.TaskGroupParent.ElementAt(4);
                        break;
                    default:
                        break;
                }

                TaskVariantCard newCard = Instantiate(_taskVariantCardPrefab, parent);
                newCard.name = $"TaskCard ({taskNumber})";
                newCard.Init(iTaskData, taskNumber);
                _taskVariantCards.Add(newCard);

                taskNumber++;
            }
        }
    }

    /// <summary>
    /// Создание связи между отдельными заданиями
    /// </summary>
    [ContextMenu("CreateConnects")]
    public void CreateConnects() {
        if (_taskVariantCards.Count < 1) Debug.LogError("TaskController: карточки с заданиями не добавлены в список");
        foreach (TaskVariantCard iTaskCard in _taskVariantCards) {
            if (iTaskCard.TaskData.NextTaskData.Count > 0) {
                foreach (TaskData iNextTaskData in iTaskCard.TaskData.NextTaskData) {
                    TaskVariantCard startTaskCard = iTaskCard;
                    TaskVariantCard nextTaskCard = _taskVariantCards.First(t => t.TaskData == iNextTaskData);
                    _taskConnectorsManager.CreateConnect(iTaskCard, nextTaskCard);
                }
            }
        }
    }
    #endregion

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

    public int FindTaskNumber(string id) {
        for (int i = 0; i < _tasksConfig.Tasks.Count(); i++) {
            string idTask = _tasksConfig.Tasks.ElementAt(i).ID;
            if (idTask == id) {
                return i;
            }
        }
        return 999;
    }

    /// <summary>
    /// Разблокирование следующих заданий
    /// </summary>
    /// <param name="signal"></param>
    private void NextTaskUnlocked(TaskFinishedSignal signal) {
        if (signal.GeneralSwitchingResult.CheckStatus) {
            signal.GeneralSwitchingResult.TaskData.SetStatus(TaskStatus.Complite);

            List<TaskData> nextTasks = signal.GeneralSwitchingResult.TaskData.NextTaskData;
            foreach (TaskData iTask in nextTasks) {
                if (iTask.TaskStatus == TaskStatus.Lock) {
                    iTask.SetStatus(TaskStatus.Unlock);
                }
            }
        }
    }

    private void SetNextTask(TaskNextSignal signal) {
        if (_currentTaskData.NextTaskData.Count > 0) {
            TaskData nextTaskData = _currentTaskData.NextTaskData[0];
            if (nextTaskData != null) {
                _currentTaskData = nextTaskData;
            }        
        } else {
            int newTaskNumber = FindTaskNumber(_currentTaskData.ID) + 1;
            if (newTaskNumber == _tasksConfig.Tasks.Count()) {
                _eventBus.Invoke(new TrainingCompliteSignal());
            } else {
                _currentTaskData = _tasksConfig.Tasks.ElementAt(newTaskNumber);
            }
        }

        _currentTaskId = _currentTaskData.ID.ToString();
        _eventBus.Invoke(new TaskSelectSignal(_currentTaskData));
    }

    private void RestartTask(RestartTaskSignal signal) {
        _eventBus.Invoke(new TaskSelectSignal(_currentTaskData));
    }

    private void SelectTask(string taskId) {
        _currentTaskId = taskId;
        _currentTaskData = _taskLoader.GetTasks().FirstOrDefault(x => x.ID == _currentTaskId);
        _eventBus.Invoke(new TaskSelectSignal(_currentTaskData));
    }
    
    private void TaskSelect(TaskStartedSignal signal) {
        _currentTaskData = signal.TaskData;
        _currentTaskId = signal.TaskData.ID.ToString();
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
        _eventBus.Unsubscribe<TaskNextSignal>(SetNextTask);
        _eventBus.Unsubscribe<TaskTimePassedSignal>(TaskTimePassed);
    }

}