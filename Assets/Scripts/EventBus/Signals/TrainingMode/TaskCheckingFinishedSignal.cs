namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал об окончании проверки
    /// </summary>
    public class TaskCheckingFinishedSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public TaskCheckingFinishedSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
