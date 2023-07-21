using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseDialog : Dialog {
    [Tooltip("������ ��� �����������")]
    [SerializeField] private Button _resumeButton;
    [Tooltip("������ ��� �������� � ���������")]
    [SerializeField] private Button _settingsButton;
    [Tooltip("������ ��� �������� � ����")]
    [SerializeField] private Button _goToMenuButton;

    private EventBus _eventBus;

    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _settingsButton.onClick.AddListener(ShowSettings);
        _resumeButton.onClick.AddListener(ResumeTask);
        _goToMenuButton.onClick.AddListener(GoToMenu);
    }

    private void ShowSettings() {
        DialogManager.ShowDialog<SettingsDialog>();
        Hide();
    }

    private void ResumeTask() {
        Debug.Log("PauseDialog: ResumeTask");
        Hide();
        _eventBus.Invoke(new TaskResumeSignal());
    }

    private void GoToMenu() {
        Hide();
        //ServiceLocator.Current.Get<TaskController>().TasksConfig.Tasks);
        _eventBus.Invoke(new TaskListCreatedSignal());
    }
}
