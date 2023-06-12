using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TaskManager : MonoBehaviour
{
    [SerializeField] private List<Task> Tasks = new List<Task>();

    public Task ShowTask(string taskName) {
        Task newTask = new Task();

        foreach (var iTask in Tasks) {
            if (iTask.Name == taskName) {
                return newTask;
            }
        }
        return null;
    }
}
