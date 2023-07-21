using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Dialogs {
    /// <summary>
    /// ���� ���������� � ������ "����������"
    /// </summary>
    public class TrainingSwitchingDialog : Dialog {
        public PrincipalSchemaView PrincipalSchemaView => _principalSchemaView;
        public SwitchBoxsSelectorView SwitchBoxsSelectorView => _switchBoxsSelectorView;
        [SerializeField] private PrincipalSchemaView _principalSchemaView;
        [SerializeField] private SwitchBoxsSelectorView _switchBoxsSelectorView;
        [SerializeField] private WagoClipsDragPanel _wagoClipsDragPanel;

        [SerializeField] private Button _pauseButton;
        [SerializeField] private Button _checkSwitching;

        private EventBus _eventBus;

        public void Init(SwitchBoxManager switchBoxManager, Stopwatch stopwatch, WagoCreator wagoCreator, EventBus eventBus) {
            _eventBus = eventBus;
            _principalSchemaView.Init(stopwatch, _eventBus);
            _wagoClipsDragPanel.Init(wagoCreator);
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
