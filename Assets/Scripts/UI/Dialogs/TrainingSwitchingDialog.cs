using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogs {
    /// <summary>
    /// Окно коммутации в модуле "Тренировка"
    /// </summary>
    public class TrainingSwitchingDialog : Dialog {
        [SerializeField] private PrincipalSchemaView _principalSchemaView;
        [SerializeField] private SwitchBoxsSelectorView _switchBoxsSelectorView;

        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _checkSwitching;

        private EventBus _eventBus;

        public void Init(SwitchBoxManager switchBoxManager, Stopwatch stopwatch) {
            _principalSchemaView.Init(stopwatch);
            _switchBoxsSelectorView.Init(switchBoxManager);
            _pauseButton.onClick.AddListener(PauseSwitching);
            _checkSwitching.onClick.AddListener(CheckSwitching);
        }

        private void PauseSwitching() {
            _eventBus.Invoke(new TaskPauseSignal());
        }

        private void CheckSwitching() {
            _eventBus.Invoke(new TaskCheckingStartSignal());
        }
    }
}
