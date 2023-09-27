using CustomEventBus.Signals;
/// <summary>
/// Отображает прогресс в модуле "Обучение"
/// </summary>
public class LearningProgressView : ProgressView {
    public override void Init() {
        base.Init();
        _eventBus.Subscribe<LearningProgressChangedSignal>(LearningProgressChanged);
    }

    private void LearningProgressChanged(LearningProgressChangedSignal signal) {
        ShowProgress(signal.LearningProgressValue, 1);
    }

    private void OnDestroy() {
        _eventBus.Unsubscribe<LearningProgressChangedSignal>(LearningProgressChanged);
    }
}
