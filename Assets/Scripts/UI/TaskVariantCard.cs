using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskVariantCard : MonoBehaviour {
    public TaskData TaskData => _taskData;
    public List<RectTransform> Points => _points;

    [SerializeField] private TaskData _taskData;
    [Header("UI Elements")]
    [SerializeField] private Button _clickerButton;
    [SerializeField] private Image _headerBackground;
    [SerializeField] private TextMeshProUGUI _taskNameText;
    [SerializeField] private StatusIconSelector _lockStatus;
    [SerializeField] private TextMeshProUGUI _statusValueText;
    [SerializeField] private GameObject _parameters;
    [SerializeField] private TextMeshProUGUI _expCountText;
    [SerializeField] private TextMeshProUGUI _coinsCountText;
    [SerializeField] private Color _headerBackgroundLockColor;
    
    [Space(5)]
    [SerializeField] private List<RectTransform> _points;

    private TaskUnlockedDialog _taskUnlocked;
    private Color _defaultColor;

    public void Init(TaskData taskData, int taskNumber) {
        _taskData = taskData;
        _expCountText.text = _taskData.ExpValue.ToString();
        _coinsCountText.text = _taskData.CoinsCount.ToString();
        _taskNameText.text = $"Задание {taskNumber}";
        _defaultColor = _headerBackground.color;

        TaskStatusChenged();

        _clickerButton.onClick.AddListener(ShowTaskDescription);
    }

    private void ShowTaskDescription() {
        Debug.Log("Выбрано задание: " + _taskData.ID);
        if (_taskData.TaskStatus == TaskStatus.Lock) {
            _taskUnlocked = DialogManager.ShowDialog<TaskUnlockedDialog>();
            _taskUnlocked.TaskUnlockedView.Init(_taskUnlocked, _taskData);
            _taskUnlocked.OnTaskUnlocked += TaskStatusChenged;
        } else {
            TaskDescriptionDialog taskDescription = DialogManager.ShowDialog<TaskDescriptionDialog>();
            taskDescription.TrainingTaskDescriptionView.Init(taskDescription, _taskData);
        }
    }

    private void TaskStatusChenged() {
        if (_taskData.TaskStatistics.Attempts.Count() > 0) {
            foreach (GeneralSwitchingResult iAttempt in _taskData.TaskStatistics.Attempts) {
                if (iAttempt.CheckStatus == true) {
                    _taskData.SetStatus(TaskStatus.Complite);
                    continue;
                }
            }
        }

        if (_taskData.TaskStatus == TaskStatus.Lock) {
            _lockStatus.SetStatus(TaskStatus.Lock);
            _headerBackground.color = _headerBackgroundLockColor;
            _statusValueText.color = _headerBackgroundLockColor;
            _statusValueText.text = "Закрыто";
            _parameters.SetActive(false);
        }
        else if (_taskData.TaskStatus == TaskStatus.Unlock) {
            _lockStatus.SetStatus(TaskStatus.Unlock);
            _headerBackground.color = _defaultColor;
            _statusValueText.color = _defaultColor;
            _statusValueText.text = "";
            _parameters.SetActive(true);
        }
        else if (_taskData.TaskStatus == TaskStatus.Complite) {
            _lockStatus.SetStatus(TaskStatus.Complite);
            _headerBackground.color = _defaultColor;
            _statusValueText.color = _defaultColor;
            _statusValueText.text = "Завершено";
            _parameters.SetActive(false);
        }
    }

    private void OnDisable() {
        if (_taskUnlocked) {
           _taskUnlocked.OnTaskUnlocked -= TaskStatusChenged;
        }
    }
}
