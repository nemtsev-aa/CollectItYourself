namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� ���������
    /// </summary>
    public class ChapterFinishedSignal {

        public readonly ChapterData ChapterData;
        public ChapterFinishedSignal(ChapterData chapterData) {
            ChapterData = chapterData;
        }
    }
}
