namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава выбрана
    /// </summary>
    public class ChapterSelectSignal {

        public readonly Description ChapterDescription;
        public ChapterSelectSignal(Description chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
