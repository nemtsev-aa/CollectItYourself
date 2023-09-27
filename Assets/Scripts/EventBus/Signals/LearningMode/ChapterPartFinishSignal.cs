namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� ����� ���������
    /// </summary>
    public class ChapterPartFinishSignal {

        public readonly ParagraphDescription ChapterPartDescription;
        public ChapterPartFinishSignal(ParagraphDescription chapterPartDescription) {
            ChapterPartDescription = chapterPartDescription;
        }
    }
}
