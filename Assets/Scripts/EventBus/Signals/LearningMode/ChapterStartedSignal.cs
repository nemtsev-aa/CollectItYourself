namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава запускается
    /// </summary>
    public class ChapterStartedSignal {

        public readonly LearningModeDescription ChapterDescription;
        public ChapterStartedSignal(LearningModeDescription chapter) {
            ChapterDescription = chapter;
        }
    }
}