namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ������������ ������ �����
    /// </summary>
    public class SwitchingUncorrectSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public SwitchingUncorrectSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
