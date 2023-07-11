namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ������� ���������
    /// </summary>
    public class TaskFinishedSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public TaskFinishedSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
