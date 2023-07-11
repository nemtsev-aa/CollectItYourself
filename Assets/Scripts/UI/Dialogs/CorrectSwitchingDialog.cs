using CustomEventBus;
using CustomEventBus.Signals;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CorrectSwitchingDialog : Dialog {
    [Tooltip("������������ ���������")]
    public ProgressView ProgressView;
    [Tooltip("������������ �����������")]
    public ResultsView ResultsView;
    [Tooltip("������ ��� �����������")]
    [SerializeField] private Button _nextTaskButton;
    [Tooltip("������ ��� �������� � ����")]
    [SerializeField] private Button _goToMenuButton;
    
    private EventBus _eventBus;
   
    public void Init(GeneralSwitchingResult result) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _nextTaskButton.onClick.AddListener(NextTask);
        _goToMenuButton.onClick.AddListener(GoToMenu);

        ResultsView.Initialize(result);
        ResultsView.UploadResultToGeneralPanel();
        ResultsView.UploadResultsToSinglePanel();
    }

    private void NextTask() {
        _eventBus.Invoke(new TaskNextSignal());
        Hide();
    }

    private void GoToMenu() {
        SceneManager.LoadScene(StringConstants.MENU_SCENE_NAME);
        Hide();
    }
}
