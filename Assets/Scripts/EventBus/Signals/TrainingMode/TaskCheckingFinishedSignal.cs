namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ �� ��������� ��������
    /// </summary>
    public class TaskCheckingFinishedSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public TaskCheckingFinishedSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
