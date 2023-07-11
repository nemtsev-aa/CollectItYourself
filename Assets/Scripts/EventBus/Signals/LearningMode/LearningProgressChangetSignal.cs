namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ������ "��������" ����������
    /// </summary>
    public class LearningProgressChangedSignal {
        public readonly int LearningProgressValue;
        public LearningProgressChangedSignal(int learningProgressValue) {
            LearningProgressValue = learningProgressValue;
        }
    }
}
