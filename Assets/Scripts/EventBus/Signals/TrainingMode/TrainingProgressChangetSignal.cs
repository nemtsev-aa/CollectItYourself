namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что количество полученного опыта в модуле "Тренировка" изменилось
    /// </summary>
    public class TrainingProgressChangedSignal {
        public readonly int CurrentExpValue;
        public readonly int FullExpValue;

        public TrainingProgressChangedSignal(int currentExpValue, int fullExpValue) {
            CurrentExpValue = currentExpValue;
            FullExpValue = fullExpValue;
        }
    }
}
