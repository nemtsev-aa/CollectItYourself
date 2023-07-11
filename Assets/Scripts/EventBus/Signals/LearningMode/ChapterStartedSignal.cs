namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� �����������
    /// </summary>
    public class ChapterStartedSignal {

        public readonly ChapterData ChapterData;
        public ChapterStartedSignal(ChapterData chapterData) {
            ChapterData = chapterData;
        }
    }
}