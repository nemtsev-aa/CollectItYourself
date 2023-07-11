namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ������� �����������
    /// </summary>
    public class TaskStartedSignal {

        public readonly TaskData TaskData;
        public TaskStartedSignal(TaskData taskData) {
            TaskData = taskData;
        }
    }
}