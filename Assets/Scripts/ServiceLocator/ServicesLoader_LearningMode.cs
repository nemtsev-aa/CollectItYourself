using CustomEventBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Dialogs;
using UnityEngine;

public class ServicesLoader_LearningMode : ServicesLoader {
    [SerializeField] private GUIHolder _guiHolder;                                          // Контейнер для всплывающих окон
                           
    private LearningProgressManager _learningProgressManager;                               // Менеджер состояния приложения в модуле "Обучение"
    private LearningProgressView _learningProgressView;                                     // Виджет прогресса в модуле "Обучение"
    private GoldController _goldController;                                                 // Менеджер золота
    private GoldCountView _goldView;                                                        // Виджет золота

    private LearningModeMenuDialog _learningModeMenuDialog;                                 // Окно выбора главы в модуле "Обучение"                     
    private ITaskLoader _levelLoader;
    private LearningModeType _currentLearningModeType;

    private void Start() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _savesManager = ServiceLocator.Current.Get<SavesManager>();
        _goldController = ServiceLocator.Current.Get<GoldController>();

        ServiceLocator.Current.RegisterWithReplacement(_guiHolder);

        ShowLearningModeMenu();
    }

    private void ShowLearningModeMenu() {
        _learningModeMenuDialog = DialogManager.ShowDialog<LearningModeMenuDialog>();
        _learningModeMenuDialog.Init(this);
        _goldView = _learningModeMenuDialog.GoldCountView;
        _goldView.Init(_eventBus, _goldController);
    }

    public void SetLearningModeType(LearningModeType type) {
        _learningModeMenuDialog.Hide();
        _currentLearningModeType = type;
        Init();
        RegisterServices();
        AddDisposables();
    }

    public override void Init() {
        
    }

    public override void RegisterServices() {
        
    }

    public override void AddDisposables() {

    }

    public override void OnDestroy() {
        
    }
}
