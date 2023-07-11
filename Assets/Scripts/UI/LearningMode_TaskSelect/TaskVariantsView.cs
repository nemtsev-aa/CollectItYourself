using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskVariantsView : MonoBehaviour {
    [Tooltip("������� - ��� ���������� �������� �������")]
    [SerializeField] private TextMeshProUGUI _selectionTaskVariantText;
    [SerializeField] private TaskVariantPanel _taskVariantPanelPrefab;
    public Action OnTaskTypeCanged; // ������� ����������� ��� ������ ���� �������
    
    [Header("View")]
    [SerializeField] private float delay = 0.1f;   // �������� ����� �����������
    
    private List<TaskData> _taskDatas; // ������ ������� ��� �����������
    private List<TaskVariantPanel> _taskVariantPanels = new List<TaskVariantPanel>(); // ������ ��������� ������� � ���������� �������
    private TaskType _currentVariant; 
    private TaskTypesView _taskTypesView; // ������ ��� ����������� ����� �������

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
