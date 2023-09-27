namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что часть главы завершена
    /// </summary>
    public class ChapterPartFinishSignal {

        public readonly ParagraphDescription ChapterPartDescription;
        public ChapterPartFinishSignal(ParagraphDescription chapterPartDescription) {
            ChapterPartDescription = chapterPartDescription;
        }
    }
}
