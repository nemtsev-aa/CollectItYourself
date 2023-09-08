using CustomEventBus;
using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UI.Dialogs;
using UnityEngine;

public class ServicesLoader_LearningMode : ServicesLoader {
    [SerializeField] private GUIHolder _guiHolder;                                          // ��������� ��� ����������� ����
                           
    private LearningProgressManager _learningProgressManager;                               // �������� ��������� ���������� � ������ "��������"
    private LearningProgressView _learningProgressView;                                     // ������ ��������� � ������ "��������"
    private GoldController _goldController;                                                 // �������� ������
    private GoldCountView _goldView;                                                        // ������ ������

    private LearningModeMenuDialog _learningModeMenuDialog;                                 // ���� ������ ����� � ������ "��������"                     
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
