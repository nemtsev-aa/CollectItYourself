namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о некорректной сборке схемы
    /// </summary>
    public class SwitchingUncorrectSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public SwitchingUncorrectSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
