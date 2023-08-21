using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Отображает прогресс в модуле "Тренировка"
/// </summary>
public class TrainingModeProgressManager : ProgressManager {
    private IEnumerable<TaskData> _taskList;
        
    public override void Init() {
        base.Init();

        _taskList = ServiceLocator.Current.Get<TaskController>().TasksConfig.Tasks;
    }

    public override void TaskFinished(TaskFinishedSignal signal) {
        if (signal.GeneralSwitchingResult.CheckStatus && signal.GeneralSwitchingResult.TaskData.TaskStatus == TaskStatus.Unlock) {
            _currentExpValue += signal.GeneralSwitchingResult.TaskData.ExpValue;
        }
        _eventBus.Invoke(new TrainingProgressChangedSignal(_currentExpValue, _fullExpAmount));
    }

    public override void ShowProgressValue(TaskListCreatedSignal signal) {
        GetCurrentProgressValue();
    }

    /// <summary>
    /// Получение текущего показателя прогресса 
    /// </summary>
    public void GetCurrentProgressValue() {
        GetCurrentExpValue();
        GetFillExpAmount();

        _eventBus.Invoke(new TrainingProgressChangedSignal(_currentExpValue, _fullExpAmount));
    }

    /// <summary>
    /// Демонстрация текущего значения прогресса
    /// </summary>
    public void ShowCurrentProgressValue() {
        _eventBus.Invoke(new TrainingProgressChangedSignal(_currentExpValue, _fullExpAmount));
    }

    /// <summary>
    /// Получение количества заработанного опыта
    /// </summary>
    private void GetCurrentExpValue() {
        if (_taskList.Count() > 0) {
            foreach (var iTask in _taskList) {
                if (iTask.TaskStatus == TaskStatus.Complite) {
                    _currentExpValue += iTask.ExpValue;
                }
            }
        }
    }

    /// <summary>
    /// Количество доступного опыта
    /// </summary>
    private void GetFillExpAmount() {
        if (_taskList.Count() > 0) {
            foreach (var iTask in _taskList) {
                _fullExpAmount += iTask.ExpValue;
            }
        }
    }

    public override void Dispose() {
        _eventBus.Unsubscribe<TaskListCreatedSignal>(ShowProgressValue);
    }

}
