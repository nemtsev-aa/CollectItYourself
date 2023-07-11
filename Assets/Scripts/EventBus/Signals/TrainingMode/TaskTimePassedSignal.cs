namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� ������� ���������
    /// </summary>
    public class TaskTimePassedSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public TaskTimePassedSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
