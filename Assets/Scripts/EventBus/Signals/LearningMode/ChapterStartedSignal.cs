namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� �����������
    /// </summary>
    public class ChapterStartedSignal {

        public readonly ChapterDescription ChapterDescription;
        public ChapterStartedSignal(ChapterDescription chapter) {
            ChapterDescription = chapter;
        }
    }
}