namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что модуль "Обучение" остановлен
    /// </summary>
    public class LearningProgressChangedSignal {
        public readonly int LearningProgressValue;
        public LearningProgressChangedSignal(int learningProgressValue) {
            LearningProgressValue = learningProgressValue;
        }
    }
}
