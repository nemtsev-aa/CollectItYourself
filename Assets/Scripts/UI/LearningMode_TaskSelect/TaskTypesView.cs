using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskTypesView : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI _selectionTaskTypeText;
    [SerializeField] private TaskVariantsView _taskVariantsView;
    [SerializeField] private TaskTypePanel _taskTypePanelPrefab;
    [SerializeField] private List<TaskTypePanel> _taskTypePanels = new List<TaskTypePanel>();
    [SerializeField] private List<TaskData> _taskDatas;
    public Action<List<TaskData>> OnTaskTypeCanged;

    private TaskType _currentType;

    private void Start() {
        Init(_taskDatas);
    }

    public void Init(List<TaskData> taskData) {
        _taskDatas = taskData;

        if (_taskDatas.Count > 0) {
            foreach (var iTask in _taskDatas) {
                TaskTypePanel foundPanel = _taskTypePanels.Find(panel => panel.GetTaskType() == iTask.Type);
                if (foundPanel == null) {
                    TaskTypePanel newPanel = Instantiate(_taskTypePanelPrefab, transform);
                    newPanel.Init(iTask);
                    newPanel.OnTaskTypeSelected += TaskTypeSelected;
                    _taskTypePanels.Add(newPanel);
                } 
            }
        }

        OnTaskTypeCanged += _taskVariantsView.Init;
    }

    private void TaskTypeSelected(TaskData task) {
        _selectionTaskTypeText.text = task.Type.ToString();
        _currentType = task.Type;

        List<TaskData> filteredList = _taskDatas.FindAll(td => td.Type == _currentType);
        OnTaskTypeCanged?.Invoke(filteredList);
    }

    private void OnEnable() {
        foreach (var iPanel in _taskTypePanels) {
            iPanel.OnTaskTypeSelected -= TaskTypeSelected;
        }
    }
}
