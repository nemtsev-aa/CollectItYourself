using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class SelectTrainingTaskDialog : Dialog {
    public TrainingProgressView TrainingProgressView { get { return _trainingProgressView; } private set { } }
    public TaskConnectorsManager TaskConnectorsManager { get { return _taskConnectorsManager; } private set { } }
    public IEnumerable<RectTransform> TaskGroupParent { get { return _taskGroupParent; } private set { } }
    public GoldCountView GoldCountView { get { return _goldCountView; } private set { } }

    [SerializeField] private TrainingProgressView _trainingProgressView;
    [SerializeField] private GoldCountView _goldCountView;
    [SerializeField] private TaskConnectorsManager _taskConnectorsManager;
    [SerializeField] private List<RectTransform> _taskGroupParent = new List<RectTransform>();

    public void Init() {
        _trainingProgressView.Init();
        _goldCountView.Init();
    }
}
