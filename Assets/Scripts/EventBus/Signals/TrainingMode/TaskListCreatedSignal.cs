using System.Collections.Generic;

namespace CustomEventBus.Signals {
    /// <summary>
    /// ������ � ���, ��� ������ ������� ������� �� �����
    /// </summary>
    public class TaskListCreatedSignal {
        public readonly List<TaskData> TaskDataList;
        public TaskListCreatedSignal(List<TaskData> taskDataList) {
            TaskDataList = taskDataList;
        }
    }
}
