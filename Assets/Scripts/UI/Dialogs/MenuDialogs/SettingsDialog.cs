using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsDialog : Dialog {
    [Tooltip(" нопка дл€ продолжени€")]
    [SerializeField] private Button _resumeButton;
    [Tooltip(" нопка дл€ перехода в меню")]
    [SerializeField] private Button _goToMenuButton;

    private EventBus _eventBus;

    public void Init(GeneralSwitchingResult result) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _resumeButton.onClick.AddListener(ResumeTask);
        _goToMenuButton.onClick.AddListener(GoToMenu);
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
