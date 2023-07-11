namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что задание завершено
    /// </summary>
    public class TaskFinishedSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public TaskFinishedSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
