using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections.Generic;
using UI;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// Выбор задания в модуле "Тренировка", режим "Сборка по вариантам"
/// </summary>
public class SelectTrainingTaskDialog : Dialog {
    public TaskConnectorsManager TaskConnectorsManager { get { return _taskConnectorsManager; } private set { } }
    public IEnumerable<RectTransform> TaskGroupParent { get { return _taskGroupParent; } private set { } }

    [Header("Navigations")]
    [SerializeField] private Button _mainMenuButton;
    [SerializeField] private Button _returnButton;
    [Header("Views")]
    [SerializeField] private TrainingProgressView _trainingProgressView;
    [SerializeField] private GoldCountView _goldCountView;
    [Header("Connections")]
    [SerializeField] private TaskConnectorsManager _taskConnectorsManager;
    [SerializeField] private List<RectTransform> _taskGroupParent = new List<RectTransform>();

    public void Init(EventBus eventBus) {
        
        _trainingProgressView.Init();
        GoldController goldController = ServiceLocator.Current.Get<GoldController>();
        _goldCountView.Init(eventBus, goldController);
        _goldCountView.ShowGoldCount();

        _mainMenuButton.onClick.AddListener(ShowMainMenu);
        _returnButton.onClick.AddListener(ReturnToModeMenu);
    }

    private void ReturnToModeMenu() {
        this.Hide();
    }

    private void ShowMainMenu() {
        ServiceLocator.Current.Get<EventBus>().Invoke(new ApplicationStateChangedSignal(ApplicationState.StartMenu));
    }
}
