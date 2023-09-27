namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава завершена
    /// </summary>
    public class ChapterFinishedSignal {

        public readonly ChapterDescription ChapterDescription;
        public ChapterFinishedSignal(ChapterDescription chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
