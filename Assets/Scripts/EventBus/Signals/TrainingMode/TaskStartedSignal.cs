namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что задание запускается
    /// </summary>
    public class TaskStartedSignal {

        public readonly TaskData TaskData;
        public TaskStartedSignal(TaskData taskData) {
            TaskData = taskData;
        }
    }
}