using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class LearningModeMenuDialog : Dialog {
    public GoldCountView GoldCountView => _goldCountView;

    [Header("Navigations")]
    [SerializeField] private Button _mainMenuButton;

    [Header("Buttons")]
    [SerializeField] private DescriptionButton _descriptionButtonPrefab;
    [SerializeField] private Transform _buttonsParent;
    [SerializeField] private List<DescriptionButton> _buttons;

    [Header("Description Window")]
    [SerializeField] private Image _descriptionImage;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Button _startButton;

    [Header("View")]
    [SerializeField] private GoldCountView _goldCountView;

    private LearningModeDescriptionSOLoader _learningModeDescriptionSOLoader;
    private IEnumerable<LearningModeDescription> _descriptions;
    
    private LearningModeDescription _currentDescription;
    private EventBus _eventBus;
    private GoldController _goldController;

    public void Init(LearningModeManager learningModeManager) {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _goldController = ServiceLocator.Current.Get<GoldController>();

        _learningModeDescriptionSOLoader = ServiceLocator.Current.Get<LearningModeDescriptionSOLoader>();
        _descriptions = _learningModeDescriptionSOLoader.Descriptions;

        _goldCountView.Init(_eventBus, _goldController);

        _mainMenuButton.onClick.AddListener(ShowMainMenu);
        _startButton.onClick.AddListener(StartMode);

        CreateDescriptionButtons();
    }

    private void CreateDescriptionButtons() {
        foreach (LearningModeDescription item in _descriptions) {
            DescriptionButton newButton = Instantiate(_descriptionButtonPrefab, _buttonsParent);
            newButton.Init(item);
            newButton.OnActivate += ShowDescription;

            _buttons.Add(newButton);
        }
        _buttons[0].Activate();
    }

    private void ShowDescription(Description description, DescriptionButton button) {
        foreach (var item in _buttons) {
            if (item != button) item.Deactivate();
        }

        _currentDescription = description as LearningModeDescription;
        _descriptionText.text = description.Text;
        _descriptionImage.sprite = description.Icon;
    }

    private void StartMode() {
        //Hide();
        _eventBus.Invoke(new ChapterSelectSignal(_currentDescription));
    }

    private void ShowMainMenu() {
        _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
    }
}

