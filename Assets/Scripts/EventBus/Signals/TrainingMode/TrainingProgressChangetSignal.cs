namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ���������� ����������� ����� � ������ "����������" ����������
    /// </summary>
    public class TrainingProgressChangedSignal {
        public readonly int CurrentExpValue;
        public readonly int FullExpValue;

        public TrainingProgressChangedSignal(int currentExpValue, int fullExpValue) {
            CurrentExpValue = currentExpValue;
            FullExpValue = fullExpValue;
        }
    }
}
