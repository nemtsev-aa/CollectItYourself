namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� ���������
    /// </summary>
    public class ChapterFinishedSignal {

        public readonly LearningModeDescription ChapterDescription;
        public ChapterFinishedSignal(LearningModeDescription chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
