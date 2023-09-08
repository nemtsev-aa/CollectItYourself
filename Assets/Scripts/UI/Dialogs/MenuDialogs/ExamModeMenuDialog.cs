using CustomEventBus;
using CustomEventBus.Signals;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.UI;

public class ExamModeMenuDialog : Dialog {
    public GoldCountView GoldCountView => _goldCountView;

    [Header("Navigations")]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _returnButton;

    [Header("Buttons")]
    [SerializeField] private List<Description> _descriptions;
    [SerializeField] private DescriptionButton _descriptionButtonPrefab;
    [SerializeField] private Transform _buttonsParent;
    [SerializeField] private List<DescriptionButton> _buttons;

    [Header("Description Window")]
    [SerializeField] private Image _descriptionImage;
    [SerializeField] private TextMeshProUGUI _descriptionText;
    [SerializeField] private Button _startButton;

    [Header("View")]
    [SerializeField] private GoldCountView _goldCountView;

    private Description _currentDescription;
    private ServicesLoader_ExamMode _services;

    public void Init(ServicesLoader_ExamMode services) {
        _services = services;

        _mainMenuButton.onClick.AddListener(ShowMainMenu);
        _returnButton.onClick.AddListener(ReturnToModeMenu);
    }

    public override void Awake() {
        base.Awake();
        foreach (Description item in _descriptions) {
            DescriptionButton newButton = Instantiate(_descriptionButtonPrefab, _buttonsParent);
            newButton.Init(item);
            newButton.OnActivate += ShowDescription;

            _buttons.Add(newButton);
        }

        _buttons[0].Activate();
        _startButton.onClick.AddListener(StartMode);
    }

    private void ShowDescription(Description description, DescriptionButton button) {
        foreach (var item in _buttons) {
            if (item != button) item.Deactivate();
        }

        _currentDescription = description;
        _descriptionText.text = description.Text;
        _descriptionImage.sprite = description.Icon;
    }

    private void StartMode() {
        ExamModeDescription description = (ExamModeDescription)_currentDescription;
        _services.SetExamModeType(description.CurrentType);
    }

    private void ReturnToModeMenu() {
        this.Hide();
    }

    private void ShowMainMenu() {
        ServiceLocator.Current.Get<EventBus>().Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
    }
}


