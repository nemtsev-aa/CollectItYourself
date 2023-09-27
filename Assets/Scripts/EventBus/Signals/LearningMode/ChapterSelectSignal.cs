namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� �������
    /// </summary>
    public class ChapterSelectSignal {

        public readonly ChapterDescription ChapterDescription;
        public ChapterSelectSignal(ChapterDescription chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
