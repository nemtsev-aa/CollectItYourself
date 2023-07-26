using CustomEventBus.Signals;

public class TrainingProgressView : ProgressView {

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
}
