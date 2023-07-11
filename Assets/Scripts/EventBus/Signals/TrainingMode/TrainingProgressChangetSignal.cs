namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что количество опыта в модуле "Тренировка" изменилось
    /// </summary>
    public class TrainingProgressChangedSignal {
        public readonly int TrainingProgressValue;
        public TrainingProgressChangedSignal(int trainingProgressValue) {
            TrainingProgressValue = trainingProgressValue;
        }
    }
}
