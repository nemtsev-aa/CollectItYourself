using CustomEventBus;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;

public class ServicesLoader_ExamMode : ServicesLoader {
    [SerializeField] private GUIHolder _guiHolder;                                          // ��������� ��� ����������� ����

    //private ExamProgressManager _learningProgressManager;                                 // �������� ��������� ���������� � ������ "�������"
    private ExamProgressView _learningProgressView;                                         // ������ ��������� � ������ "��������"
    private GoldController _goldController;                                                 // �������� ������
    private GoldCountView _goldView;                                                        // ������ ������

    private ExamModeMenuDialog _examModeMenuDialog;                                 // ���� ������ ����� � ������ "��������"                     
    private ITaskLoader _levelLoader;
    private ExamModeType _currentExamModeType;

    private void Start() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _savesManager = ServiceLocator.Current.Get<SavesManager>();
        _goldController = ServiceLocator.Current.Get<GoldController>();

        ServiceLocator.Current.RegisterWithReplacement(_guiHolder);

        ShowExamModeMenu();
    }

    private void ShowExamModeMenu() {
        _examModeMenuDialog = DialogManager.ShowDialog<ExamModeMenuDialog>();
        _examModeMenuDialog.Init(this);
        _goldView = _examModeMenuDialog.GoldCountView;
        _goldView.Init(_eventBus, _goldController);
    }

    public void SetExamModeType(ExamModeType type) {
        _examModeMenuDialog.Hide();
        _currentExamModeType = type;
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
