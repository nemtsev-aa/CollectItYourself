namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ����� �����������
    /// </summary>
    public class ChapterStartedSignal {

        public readonly LearningModeDescription ChapterDescription;
        public ChapterStartedSignal(LearningModeDescription chapter) {
            ChapterDescription = chapter;
        }
    }
}