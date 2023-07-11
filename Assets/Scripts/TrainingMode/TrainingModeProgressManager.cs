using CustomEventBus;
using CustomEventBus.Signals;

/// <summary>
/// ���������� �������� � ������ "����������"
/// ���������� � ������/����� ����
/// </summary>
public class TrainingModeProgressManager : ProgressManager, IService, IDisposable {
    /// <summary>
    /// ������� ���������
    /// </summary>
    /// <param name="taskData"></param>
    private void TaskComplite(TaskData taskData) {
        //CurrentExpValue += taskData.ExpAmountToComplete;
        int _currentProgress = (CurrentExpValue / _fullExpAmount) * 100;
        _eventBus.Invoke(new TrainingProgressChangedSignal(_currentProgress));
    }

    /// <summary>
    /// ������ "����������" ��������
    /// </summary>
    private void TrainingComplite(TrainingCompliteSignal signal) {
        _eventBus.Invoke(new TrainingCompliteSignal());
    }
}
