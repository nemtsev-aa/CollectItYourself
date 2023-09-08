using CustomEventBus;
using CustomEventBus.Signals;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class TheoreticalPartDialog : Dialog {
    [Header("Navigations")]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _nextButton;

    [Header("Description Window")]
    [SerializeField] private MyVideoPlayer _descriptionPlayer;
    [SerializeField] private TextMeshProUGUI _descriptionText;

    private LearningModeManager _learningModeManager;
    private LearningModeDescription _currentDescription;
    private EventBus _eventBus;

    public void Init(Description chapterDescription, LearningModeManager learningModeManager) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _learningModeManager = learningModeManager;
        _currentDescription = chapterDescription as LearningModeDescription;

        _descriptionPlayer.Init(_currentDescription.VideoClip);
        _descriptionText.text = _currentDescription.Text;

        _mainMenuButton.onClick.AddListener(ShowMainMenu);
        _returnButton.onClick.AddListener(ReturnToModeMenu);
        _nextButton.onClick.AddListener(GoToPlacticalPart);
    }

    private void ReturnToModeMenu() {
        _learningModeManager.ReturnToLearningModeMenu();
    }

    private void GoToPlacticalPart() {
        _learningModeManager.ShowPlacticalPart();
    }

    private void ShowMainMenu() {
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
    }
}
