using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TaskTypePanel : MonoBehaviour {
    [SerializeField] private TaskData _currentTask;
    [SerializeField] private TextMeshProUGUI _taskTypeNameText;
    
    public Action<TaskData> OnTaskTypeSelected;
    public void Init(TaskData task) {
        _currentTask = task;
        _taskTypeNameText.text = _currentTask.Type.ToString();
        transform.GetComponent<Button>().onClick.AddListener(TaskTypeSelection);
    }

    private void TaskTypeSelection() {
        OnTaskTypeSelected?.Invoke(_currentTask);
    }

    public TaskType GetTaskType() {
        return _currentTask.Type;
    }
    public TaskMode GetTaskMode() {
        return _currentTask.Mode;
    }
}
