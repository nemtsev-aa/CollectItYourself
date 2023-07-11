using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskVariantsView : MonoBehaviour {
    [Tooltip("Надпись - имя выбранного варианта задания")]
    [SerializeField] private TextMeshProUGUI _selectionTaskVariantText;
    [SerializeField] private TaskVariantPanel _taskVariantPanelPrefab;
    public Action OnTaskTypeCanged; // Событие возникающее при выборе типа задания
    
    [Header("View")]
    [SerializeField] private float delay = 0.1f;   // Задержка между появлениями
    
    private List<TaskData> _taskDatas; // Список заданий для отображения
    private List<TaskVariantPanel> _taskVariantPanels = new List<TaskVariantPanel>(); // Список созданных панелей с вариантами заданий
    private TaskType _currentVariant; 
    private TaskTypesView _taskTypesView; // Виджет для отображения типов заданий

    public void Init(List<TaskData> taskData) {
        _taskDatas = taskData;

        if (_taskDatas.Count > 0) {
            if (transform.childCount > 0) RemoveVariantsPanel();

            CreateVariantsPanel();
        }
    }
    private void CreateVariantsPanel() {
        StartCoroutine(SpawnObjects());
    }

    private void RemoveVariantsPanel() {
        foreach (var iPanel in _taskVariantPanels) {
            iPanel.OnTaskVariantSelected -= TaskVariantSelected;
            Destroy(iPanel.gameObject);
        }
        _taskVariantPanels.Clear();
    }

    private void TaskVariantSelected(TaskData task) {
        _selectionTaskVariantText.text = task.Variant.ToString();
    }

    private void OnEnable() {
        foreach (var iPanel in _taskVariantPanels) {
            iPanel.OnTaskVariantSelected -= TaskVariantSelected;
        }
    }

    private IEnumerator SpawnObjects() {
        for (int i = 0; i < _taskDatas.Count; i++) {
            yield return new WaitForSeconds(delay);
            TaskData iTask = _taskDatas[i];
            TaskVariantPanel newPanel = Instantiate(_taskVariantPanelPrefab, transform);
            newPanel.Init(iTask);
            newPanel.OnTaskVariantSelected += TaskVariantSelected;
            _taskVariantPanels.Add(newPanel);
        }
    }
}
