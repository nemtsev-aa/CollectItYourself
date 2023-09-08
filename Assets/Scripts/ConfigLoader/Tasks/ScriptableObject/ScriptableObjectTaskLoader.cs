using System.Collections.Generic;
using CustomEventBus;
using CustomEventBus.Signals;
using UnityEngine;

public class ScriptableObjectTaskLoader : MonoBehaviour, ITaskLoader {
    [SerializeField] private TasksConfig _tasksConfig;

    public IEnumerable<TaskData> GetTasks() {
        return _tasksConfig.Tasks;
    }

    public bool IsLoaded() {
        return true;
    }

    public void Load() {
        var eventBus = ServiceLocator.Current.Get<EventBus>();
        eventBus.Invoke(new DataLoadedSignal(this));
    }
    
    public bool IsLoadingInstant() {
        return true;
    }
}