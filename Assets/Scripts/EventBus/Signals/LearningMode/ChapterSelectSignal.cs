namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� �������
    /// </summary>
    public class ChapterSelectSignal {

        public readonly Description ChapterDescription;
        public ChapterSelectSignal(Description chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
