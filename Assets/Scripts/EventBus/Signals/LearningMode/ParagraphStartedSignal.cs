namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что часть главы запущена
    /// </summary>
    public class ParagraphStartedSignal {

        public readonly ParagraphDescription ParagraphDescription;
        public ParagraphStartedSignal(ParagraphDescription paragraphDescription) {
            ParagraphDescription = paragraphDescription;
        }
    }
}
