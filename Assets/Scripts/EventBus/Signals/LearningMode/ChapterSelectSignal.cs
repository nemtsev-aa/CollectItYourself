namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� �������
    /// </summary>
    public class ChapterSelectSignal {

        public readonly ChapterData ChapterData;
        public ChapterSelectSignal(ChapterData chapterData) {
            ChapterData = chapterData;
        }
    }
}
