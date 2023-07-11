namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���������� ������ �����
    /// </summary>
    public class SwitchingCorrectSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public SwitchingCorrectSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
