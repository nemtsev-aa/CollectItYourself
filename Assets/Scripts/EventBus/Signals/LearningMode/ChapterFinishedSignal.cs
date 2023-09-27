namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� ���������
    /// </summary>
    public class ChapterFinishedSignal {

        public readonly ChapterDescription ChapterDescription;
        public ChapterFinishedSignal(ChapterDescription chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
