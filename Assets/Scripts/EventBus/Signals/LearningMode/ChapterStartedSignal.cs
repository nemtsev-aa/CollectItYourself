namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава запускается
    /// </summary>
    public class ChapterStartedSignal {

        public readonly ChapterData ChapterData;
        public ChapterStartedSignal(ChapterData chapterData) {
            ChapterData = chapterData;
        }
    }
}