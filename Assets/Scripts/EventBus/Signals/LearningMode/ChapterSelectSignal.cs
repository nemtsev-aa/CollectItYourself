namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава выбрана
    /// </summary>
    public class ChapterSelectSignal {

        public readonly LearningModeDescription ChapterDescription;
        public ChapterSelectSignal(LearningModeDescription chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
