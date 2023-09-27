using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Меню главы
/// </summary>
public class ChapterMenuDialog : Dialog {
    [Header("Navigations")]
    [SerializeField] private Button _returnButton;
    [SerializeField] private Button _mainMenuButton;
    [Header("View")]
    [SerializeField] private LearningProgressView _progressView;
    [SerializeField] private GoldCountView _goldCountView;
    [Header("Companents")]
    [SerializeField] private TextMeshProUGUI _chapterNameText;
    [Header("ParagraphPanels")]
    [SerializeField] private ParagraphPanel _paragraphPanelPrefab;
    [SerializeField] private RectTransform _panelsParent;
    [SerializeField] private List<ParagraphPanel> _paragraphPanels;
    
    private LearningModeManager _learningModeManager;
    private EventBus _eventBus;
    private ChapterDescription _currentDescription;
    
    public void Init(ChapterDescription currentDescription, LearningModeManager learningModeManager) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _learningModeManager = learningModeManager;
        _currentDescription = currentDescription;

        _mainMenuButton.onClick.AddListener(ShowMainMenu);
        _returnButton.onClick.AddListener(ReturnToModeMenu);

        CreateParagraphPanels();
        ShowProgressInPanels();
    }

    private void CreateParagraphPanels() {
        foreach (var paragraphDescription in _currentDescription.ParagraphDescriptions) {
            ParagraphPanel newPanel = Instantiate(_paragraphPanelPrefab, _panelsParent);
            newPanel.Init(_learningModeManager, paragraphDescription);
            _paragraphPanels.Add(newPanel);
        }
    }

    private void ShowProgressInPanels() {
        foreach (var iPanel in _paragraphPanels) {
            ShowProgressInPanel(iPanel.Description.CurrentType);
        }
    }

    public void ShowProgressInPanel(ParagraphType type) {
        ParagraphPanel panel = _paragraphPanels.Find(t => t.Description.CurrentType == type);
        panel.ShowStatus();
    }

    private void ReturnToModeMenu() {
        _learningModeManager.ReturnToChapterMenu(this);
    }

    private void ShowMainMenu() {
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
    }
}
