namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о корректной сборке схемы
    /// </summary>
    public class SwitchingCorrectSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public SwitchingCorrectSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
