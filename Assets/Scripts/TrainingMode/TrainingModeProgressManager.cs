using CustomEventBus;
using CustomEventBus.Signals;

/// <summary>
/// Отображает прогресс в модуле "Тренировка"
/// Уведомляет о старте/конце игры
/// </summary>
public class TrainingModeProgressManager : ProgressManager, IService, IDisposable {
    /// <summary>
    /// Задание выполнено
    /// </summary>
    /// <param name="taskData"></param>
    private void TaskComplite(TaskData taskData) {
        //CurrentExpValue += taskData.ExpAmountToComplete;
        int _currentProgress = (CurrentExpValue / _fullExpAmount) * 100;
        _eventBus.Invoke(new TrainingProgressChangedSignal(_currentProgress));
    }

    /// <summary>
    /// Модуль "Тренировка" завершён
    /// </summary>
    private void TrainingComplite(TrainingCompliteSignal signal) {
        _eventBus.Invoke(new TrainingCompliteSignal());
    }
}
