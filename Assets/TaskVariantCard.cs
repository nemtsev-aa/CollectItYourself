using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TaskVariantCard : MonoBehaviour {
    public List<RectTransform> Points => _points;
    public IEnumerable<TaskVariantCard> NextTaskCards => _nextTaskCards;

    [SerializeField] private TaskData _taskData;
    [Header("UI Elements")]
    [SerializeField] private Button _clickerButton;
    [SerializeField] private TextMeshProUGUI _taskNameText;
    [SerializeField] private Toggle _lockStatusToogle;
    [SerializeField] private TextMeshProUGUI _expCountText;
    [SerializeField] private TextMeshProUGUI _coinsCountText;
    [Space(5)]
    [SerializeField] private List<RectTransform> _points;
    [Space(5)]
    [SerializeField] private List<TaskVariantCard> _nextTaskCards;

    private GameObject _taskDescriptionPopup;


    [ContextMenu("Init")]
    public void Init(TaskData taskData) {
        _taskData = taskData;
        _expCountText.text = _taskData.ExpValue.ToString();
        _clickerButton.onClick.AddListener(ShowTaskDescription);
    }

    private void ShowTaskDescription() {
        //_taskDescriptionView.SetActive(true);
        //_taskDescriptionPopup.Init(_taskData);
    }

    public void Unlock() {
        _lockStatusToogle.isOn = false;
    }
}
