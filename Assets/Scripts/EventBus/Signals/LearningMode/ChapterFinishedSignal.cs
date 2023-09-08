namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава завершена
    /// </summary>
    public class ChapterFinishedSignal {

        public readonly LearningModeDescription ChapterDescription;
        public ChapterFinishedSignal(LearningModeDescription chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
