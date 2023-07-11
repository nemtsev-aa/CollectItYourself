namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что время задания закончено
    /// </summary>
    public class TaskTimePassedSignal {
        public readonly GeneralSwitchingResult GeneralSwitchingResult;
        public TaskTimePassedSignal(GeneralSwitchingResult generalSwitchingResult) {
            GeneralSwitchingResult = generalSwitchingResult;
        }
    }
}
