namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава завершена
    /// </summary>
    public class ChapterFinishedSignal {

        public readonly ChapterData ChapterData;
        public ChapterFinishedSignal(ChapterData chapterData) {
            ChapterData = chapterData;
        }
    }
}
