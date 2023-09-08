using CustomEventBus;
using CustomEventBus.Signals;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI.Dialogs {
    /// <summary>
    /// Окно меню (используется на титульной сцене)
    /// </summary>
    public class MainMenuDialog : Dialog {
        [SerializeField] private Button _learningButton;
        [SerializeField] private Button _trainingButton;
        [SerializeField] private Button _examButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private Button _quitButton;

        private EventBus _eventBus;
        public override void Awake() {
            base.Awake();
            _eventBus = ServiceLocator.Current.Get<EventBus>();

            _learningButton.onClick.AddListener(OnLearningButtonClick);
            _trainingButton.onClick.AddListener(OnTrainingButtonClick);
            _examButton.onClick.AddListener(OnExamButtonClick);
            _settingsButton.onClick.AddListener(OnSettingsClick);
            _quitButton.onClick.AddListener(OnQuitClick);
        }

        private void OnLearningButtonClick() {
            _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.LearningMode));
        }

        private void OnTrainingButtonClick() {
            SceneManager.LoadScene(2);
            _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.TrainingMode));
        }

        private void OnExamButtonClick() {
            SceneManager.LoadScene(3);
            _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.ExamMode));
        }

        private void OnSettingsClick() {
            _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.Settings));
        }

        private void OnQuitClick() {
            _eventBus.Invoke(new ApplicationStateChangedSignal(ApplicationState.Quit));
        }
    }
}
