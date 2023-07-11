namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава выбрана
    /// </summary>
    public class ChapterSelectSignal {

        public readonly ChapterData ChapterData;
        public ChapterSelectSignal(ChapterData chapterData) {
            ChapterData = chapterData;
        }
    }
}
