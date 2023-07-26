using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class SwitchingResultDialog : Dialog {
    [Tooltip("Визуализатор прогресса")]
    [SerializeField] private TrainingProgressView _progressView;
    [Tooltip("Визуализатор количества монет")]
    [SerializeField] private GoldCountView _goldCountView;
    [Tooltip("Визуализатор результатов")]
    [SerializeField] private ResultsView _resultsView;
    [Tooltip("Электробот")]
    [SerializeField] protected ElectroBotView _electroBotView;
    [Tooltip("Кнопка для продолжения")]
    [SerializeField] private Button _nextTaskButton;
    [Tooltip("Кнопка для перехода в меню")]
    [SerializeField] private Button _goToMenuButton;
    
    private EventBus _eventBus;

    public virtual void Init(GeneralSwitchingResult result) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _nextTaskButton.onClick.AddListener(NextTask);
        _goToMenuButton.onClick.AddListener(GoToMenu);

        _resultsView.Initialize(result);
        _resultsView.UploadResultToGeneralPanel();
        _resultsView.UploadResultsToSinglePanel();

        _goldCountView.Init();
        _progressView.Init();
    }

    private void NextTask() {
        _eventBus.Invoke(new TaskNextSignal());
        Hide();
    }

    private void GoToMenu() {
        Hide();
        _eventBus.Invoke(new TaskListCreatedSignal());
    }
}
