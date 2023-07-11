using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseDialog : Dialog {
    [Tooltip("Кнопка для продолжения")]
    [SerializeField] private Button _resumeButton;
    [Tooltip("Кнопка для перехода в настройки")]
    [SerializeField] private Button _settingsButton;
    [Tooltip("Кнопка для перехода в меню")]
    [SerializeField] private Button _goToMenuButton;

    private EventBus _eventBus;

    public void Init(GeneralSwitchingResult result) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _settingsButton.onClick.AddListener(ShowSettings);
        _resumeButton.onClick.AddListener(ResumeTask);
        _goToMenuButton.onClick.AddListener(GoToMenu);
    }

    private void ShowSettings() {
        DialogManager.ShowDialog<SettingsDialog>();
    }

    private void ResumeTask() {
        _eventBus.Invoke(new TaskResumeSignal());
        Hide();
    }

    private void GoToMenu() {
        SceneManager.LoadScene(StringConstants.MENU_SCENE_NAME);
        Hide();
    }
}
