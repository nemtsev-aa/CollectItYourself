using System.Collections.Generic;

/// <summary>
/// Загрузчик уровней из разных мест(ScriptableObject, JSON, Server)
/// </summary>
public interface ITaskLoader : IService, ILoader {   
    public IEnumerable<TaskData> GetTasks();
}