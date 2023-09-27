namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что глава запускается
    /// </summary>
    public class ChapterStartedSignal {

        public readonly ChapterDescription ChapterDescription;
        public ChapterStartedSignal(ChapterDescription chapter) {
            ChapterDescription = chapter;
        }
    }
}