using CustomEventBus;
using CustomEventBus.Signals;

public class TrainingProgressView : ProgressView, IDisposable {

    public override void Init() {
        base.Init();
        TrainingModeProgressManager progressManager = ServiceLocator.Current.Get<TrainingModeProgressManager>();
        ShowProgress(progressManager.CurrentExpValue, progressManager.FullExpAmount);
        
        _eventBus.Subscribe<TrainingProgressChangedSignal>(ShowTrainingProgress);
    }

    private void ShowTrainingProgress(TrainingProgressChangedSignal signal) {
        ShowProgress(signal.CurrentExpValue, signal.FullExpValue);
    }

    public override void ShowProgress(int currentValue, int maxValue) {
        base.ShowProgress(currentValue, maxValue);
    }

    public void Dispose() {
        _eventBus.Unsubscribe<TrainingProgressChangedSignal>(ShowTrainingProgress);
    }
}
