using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IncorrectSwitchingDialog : Dialog {
    [Tooltip("Визуализатор прогресса")]
    public ProgressView ProgressView;
    [Tooltip("Визуализатор результатов")]
    public ResultsView ResultsView;
    [Tooltip("Кнопка для перезагрузки задания")]
    [SerializeField] private Button _resetTaskButton;
    [Tooltip("Кнопка для перехода в меню")]
    [SerializeField] private Button _goToMenuButton;

    private EventBus _eventBus;

    public void Init(GeneralSwitchingResult result) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _resetTaskButton.onClick.AddListener(ResetTask);
        _goToMenuButton.onClick.AddListener(GoToMenu);

        ResultsView.Initialize(result);
        ResultsView.UploadResultToGeneralPanel();
        ResultsView.UploadResultsToSinglePanel();
    }

    private void ResetTask() {
        _eventBus.Invoke(new TaskResetSignal());
        Hide();
    }

    private void GoToMenu() {
        SceneManager.LoadScene(StringConstants.MENU_SCENE_NAME);
        Hide();
    }
}
