using System.Collections.Generic;

namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что список заданий получен из файла
    /// </summary>
    public class TaskListCreatedSignal {
        public readonly List<TaskData> TaskDataList;
        public TaskListCreatedSignal(List<TaskData> taskDataList) {
            TaskDataList = taskDataList;
        }
    }
}
