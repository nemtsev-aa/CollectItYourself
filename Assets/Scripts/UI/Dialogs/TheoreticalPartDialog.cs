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
    private TheoreticalPartDescription _currentDescription;
    private EventBus _eventBus;

    public void Init(TheoreticalPartDescription chapterDescription, LearningModeManager learningModeManager) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();

        _learningModeManager = learningModeManager;
        _currentDescription = chapterDescription;

        _descriptionPlayer.Init(_currentDescription.TheoreticalVideoClip);
        _descriptionText.text = "";

        _mainMenuButton.onClick.AddListener(ShowMainMenu);
        _returnButton.onClick.AddListener(ReturnToModeMenu);
        _nextButton.onClick.AddListener(NextStep);
    }

    private void NextStep() {
        _eventBus.Invoke(new ChapterPartFinishSignal(_currentDescription));
    }

    private void ReturnToModeMenu() {
        _learningModeManager.ReturnToLearningModeMenu(this);
    }

    private void ShowMainMenu() {
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
    }
}
