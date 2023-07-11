using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class TaskVariantPanel : MonoBehaviour {
    [SerializeField] private string _pref = "Вариант";
    [SerializeField] private TextMeshProUGUI _taskVariantNameText;
    
    public Action<TaskData> OnTaskVariantSelected;

    private TaskData _currentTask;
    public void Init(TaskData task) {
        _currentTask = task;
        _taskVariantNameText.text = _pref + " " + _currentTask.Variant.ToString();
        transform.GetComponent<Button>().onClick.AddListener(TaskTypeSelection);
    }

    private void TaskTypeSelection() {
        OnTaskVariantSelected?.Invoke(_currentTask);
    }
}
