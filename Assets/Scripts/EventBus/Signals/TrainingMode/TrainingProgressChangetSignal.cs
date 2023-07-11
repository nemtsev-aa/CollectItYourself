namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ���������� ����� � ������ "����������" ����������
    /// </summary>
    public class TrainingProgressChangedSignal {
        public readonly int TrainingProgressValue;
        public TrainingProgressChangedSignal(int trainingProgressValue) {
            TrainingProgressValue = trainingProgressValue;
        }
    }
}
