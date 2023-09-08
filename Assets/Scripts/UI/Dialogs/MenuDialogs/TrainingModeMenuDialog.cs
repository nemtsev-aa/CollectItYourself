using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogs {
    /// <summary>
    /// Окно выбора режима модуля "Тренировка"
    /// </summary>
    public class TrainingModeMenuDialog : Dialog {
        public GoldCountView GoldCountView => _goldCountView;

        [Header("Navigations")]
        [SerializeField] private Button _mainMenuButton;

        [Header("Buttons")]
        [SerializeField] private DescriptionButton _trainingModeDescriptionButtonPrefab;
        [SerializeField] private Transform _buttonsParent;
        [SerializeField] private List<DescriptionButton> _buttons;
        
        [Header("Description Window")]
        [SerializeField] private Image _descriptionImage;
        [SerializeField] private TextMeshProUGUI _descriptionText;
        [SerializeField] private Button _startButton;

        [Header("View")]
        [SerializeField] private GoldCountView _goldCountView;
        
        private Description _currentDescription;
        private ServicesLoader_TrainingMode _services;
        private TrainingModeDescriptionSOLoader _trainingModeDescriptionSOLoader;
        private IEnumerable<TrainingModeDescription> _descriptions;

        public void Init(ServicesLoader_TrainingMode services) {
            _services = services;
            _trainingModeDescriptionSOLoader = ServiceLocator.Current.Get<TrainingModeDescriptionSOLoader>();
            _descriptions = _trainingModeDescriptionSOLoader.Descriptions;

            _mainMenuButton.onClick.AddListener(ShowMainMenu);
            _startButton.onClick.AddListener(StartMode);

            CreateDescriptionButtons();
        }

        private void CreateDescriptionButtons() {
            foreach (Description item in _descriptions) {
                DescriptionButton newButton = Instantiate(_trainingModeDescriptionButtonPrefab, _buttonsParent);
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

            _currentDescription = description;
            _descriptionText.text = description.Text;
            _descriptionImage.sprite = description.Icon;
        }

        private void StartMode() {
            TrainingModeDescription description = (TrainingModeDescription)_currentDescription;
            _services.SetTrainingModeType(description.CurrentType);
        }

        private void ShowMainMenu() {
            ServiceLocator.Current.Get<EventBus>().Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
        }
    }
}
