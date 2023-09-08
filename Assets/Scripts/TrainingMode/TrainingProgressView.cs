using CustomEventBus.Signals;

public class TrainingProgressView : ProgressView {

    public override void Init() {
        base.Init();
        
        ShowProgress(_progressManager.CurrentExpValue, _progressManager.FullExpAmount);
        _eventBus.Subscribe<TrainingProgressChangedSignal>(ShowTrainingProgress);
    }

    private void ShowTrainingProgress(TrainingProgressChangedSignal signal) {
        ShowProgress(signal.CurrentExpValue, signal.FullExpValue);
    }

    public override void ShowProgress(int currentValue, int maxValue) {
        base.ShowProgress(currentValue, maxValue);
    }

    private void OnDisable() {
        _eventBus.Unsubscribe<TrainingProgressChangedSignal>(ShowTrainingProgress);
    }
}
