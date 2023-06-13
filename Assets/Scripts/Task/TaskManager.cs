using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    public Task CurrentTask;
    [SerializeField] private List<Task> Tasks = new List<Task>();

    public Task FindTask(string taskName) {
        
        foreach (var iTask in Tasks) {
            if (iTask.Name == taskName) {
                CurrentTask = iTask;
                return iTask;
            }
        }
        CurrentTask = null;
        return null;
    }
}
