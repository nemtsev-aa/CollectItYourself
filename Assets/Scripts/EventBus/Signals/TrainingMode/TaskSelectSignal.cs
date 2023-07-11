namespace CustomEventBus.Signals {
    /// <summary>
    /// Сигнал о том, что задание выбрано
    /// </summary>
    public class TaskSelectSignal {

        public readonly TaskData TaskData;
        public TaskSelectSignal(TaskData chapterData) {
            TaskData = chapterData;
        }
    }
}
