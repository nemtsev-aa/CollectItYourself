namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава выбрана
    /// </summary>
    public class ChapterSelectSignal {

        public readonly ChapterDescription ChapterDescription;
        public ChapterSelectSignal(ChapterDescription chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
