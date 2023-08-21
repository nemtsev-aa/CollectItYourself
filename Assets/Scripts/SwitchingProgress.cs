using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

public class SwitchingProgress : MonoBehaviour {
    public int Max => _max;
    public int Current => _current;

    [SerializeField] private SwitchingProgressView _progressView;
    private int _max;
    private int _current;
    private SwitchBoxesManager _switchBoxManager;
    private EventBus _eventBus;

    public void Init(SwitchBoxesManager switchBoxesManager, EventBus eventBus) {
        _switchBoxManager = switchBoxesManager;
        _eventBus = eventBus;
        _eventBus.Subscribe<ActiveSwitchBoxChangedSignal>(GetCurrentProgressValue);
    }

    /// <summary>
    /// Отображение текущих значений прогресса сборки при переключении активной распределительной коробки
    /// </summary>
    /// <param name="signal"></param>
    private void GetCurrentProgressValue(ActiveSwitchBoxChangedSignal signal) {
        if (_switchBoxManager.TaskData.Type == TaskType.Full) SetMax(_switchBoxManager.TaskData.GetConnectionsCountInSwitchBox(signal.SwitchBox.Number - 1));
        else SetMax(_switchBoxManager.TaskData.GetConnectionsCountInSwitchBox(0));

        SetCurrent(_switchBoxManager.ActiveSwichBox.GetConnectionsCount());
    }

    public void SetMax(int max) {
        _max = max;
        UpdateProgress();
    }

    public void SetCurrent(int current) {
        _current = current;
        UpdateProgress();
    }

    public void ProgressChanged(bool status) {
        if (status) ApplyProgress();
        else RemoveProgress();
    }


    public void ApplyProgress() {
        _current = _switchBoxManager.ActiveSwichBox.GetConnectionsCount();
        if (_current > _max) _current = _max;
        UpdateProgress();
    }

    public void RemoveProgress() {
        _current = _switchBoxManager.ActiveSwichBox.GetConnectionsCount();
        if (_current < 0) _current = 0;
        UpdateProgress();
    }

    private void UpdateProgress() {
        _progressView.UpdateProgress(_max, _current);
    }
}
