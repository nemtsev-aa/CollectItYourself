namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� �������
    /// </summary>
    public class ChapterSelectSignal {

        public readonly LearningModeDescription ChapterDescription;
        public ChapterSelectSignal(LearningModeDescription chapterDescription) {
            ChapterDescription = chapterDescription;
        }
    }
}
