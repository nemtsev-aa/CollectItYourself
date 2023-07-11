namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ������� �������
    /// </summary>
    public class TaskSelectSignal {

        public readonly TaskData TaskData;
        public TaskSelectSignal(TaskData chapterData) {
            TaskData = chapterData;
        }
    }
}
