using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Linq;
using UI;
using UI.Dialogs;
using UnityEngine;

/// <summary>
/// �������� �� ������ ������ "��������":
/// ����������� ����� 
/// ���������� ��������� ������� ��� ������� ����� ����������
/// ���������� ��� ������ �������
/// </summary>
public class LearningModeManager : IService, CustomEventBus.IDisposable {
    [field: SerializeField] public int CurrentExpValue { get; private set; }
    private LearningProgressView _learningProgressView;                                             // ������ ��������� � ������ "��������"
    
    private LearningModeMenuDialog _learningModeMenuDialog;                                         // ���� ������ ����� � ������ "��������"
    private ChapterMenuDialog _chapterMenuDialog;                                                   // ���� ������ ����� ����� � ������ "��������"
    private TheoreticalPartDialog _theoreticalPartDialog;                                           // ���� ������������� ����� � ������ "��������"
    private PracticalPartDialog _practicalPartDialog;                                               // ���� ������������ ����� � ������ "��������"
    private VerificationPartDialog _verificationPartDialog;                                         // ���� ����������� ����� � ������ "��������"

    private ChapterDescription _currentDescription;
    private int _fullExpAmount;
    private EventBus _eventBus;
 
    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ChapterSelectSignal>(SetChapter);
        _eventBus.Subscribe<ChapterFinishedSignal>(ChapterFinished);
        _eventBus.Subscribe<ParagraphStartedSignal>(StartParagraph);
        _eventBus.Subscribe<ChapterPartFinishSignal>(ChapterPartFinish);
    }
    
    #region Show Dialogs
    /// <summary>
    /// ���� ����� (���������� �����)
    /// </summary>
    private void ShowChapterMenuDialog() {
        _chapterMenuDialog = DialogManager.ShowDialog<ChapterMenuDialog>();
        _chapterMenuDialog.Init(_currentDescription, this);
    }
    /// <summary>
    /// ������� � ������������� ����� �����
    /// </summary>
    public void ShowTheoreticalPart(ParagraphDescription paragraphDescription) {
        if (_chapterMenuDialog != null) _chapterMenuDialog.Hide();

        _theoreticalPartDialog = DialogManager.ShowDialog<TheoreticalPartDialog>();
        _theoreticalPartDialog.Init(paragraphDescription as TheoreticalPartDescription, this);
    }

    /// <summary>
    /// ������� � ����������� ����� �����
    /// </summary>
    public void ShowPlacticalPart() {
        if (_theoreticalPartDialog != null) _theoreticalPartDialog.Hide();

        _practicalPartDialog = DialogManager.ShowDialog<PracticalPartDialog>();
        _practicalPartDialog.Init(_currentDescription.ParagraphDescriptions.ElementAt(1) as PracticalPartDescription, this);
    }

    /// <summary>
    /// ������� � �������� ����� �����
    /// </summary>
    public void ShowVerificationPart() {
        if (_practicalPartDialog != null) _practicalPartDialog.Hide();

        _verificationPartDialog = DialogManager.ShowDialog<VerificationPartDialog>();
        _verificationPartDialog.Init(_currentDescription.ParagraphDescriptions.ElementAt(2) as VerificationPartDescription, this);
    }

    /// <summary>
    /// ������� � ���� ������ ����� � ������ "��������"
    /// </summary>
    /// <param name="dialog"></param>
    public void ReturnToLearningModeMenu(Dialog dialog) {
        dialog.Hide();
        ShowLearningModeMenu();
    }

    /// <summary>
    /// ������� � ���������� ������ "��������"
    /// </summary>
    public void ShowLearningModeMenu() {
        _learningModeMenuDialog = DialogManager.ShowDialog<LearningModeMenuDialog>();
        _learningModeMenuDialog.Init(this);
    }

    /// <summary>
    /// ������� � ���������� �����
    /// </summary>
    /// <param name="currentDialog"></param>
    public void ReturnToChapterMenu(Dialog currentDialog) {
        currentDialog.Hide();
        ShowChapterMenuDialog();
    }
    #endregion

    #region Chapter Managment
    /// <summary>
    /// ����� �������
    /// </summary>
    /// <param name="signal"></param>
    private void SetChapter(ChapterSelectSignal signal) {
        _learningModeMenuDialog.Hide();
        _currentDescription = signal.ChapterDescription;
        ShowChapterMenuDialog();
    }

    /// <summary>
    /// ����� ���������
    /// </summary>
    /// <param name="signal"></param>
    private void ChapterFinished(ChapterFinishedSignal signal) {
        CurrentExpValue += signal.ChapterDescription.ExpAmountToComplete;
        int _currentProgress = (CurrentExpValue / _fullExpAmount) * 100;
        _eventBus.Invoke(new LearningProgressChangedSignal(_currentProgress));

        ShowLearningModeMenu();
    }

    #endregion

    #region ChapterPart Managment
    /// <summary>
    /// ����� ����� ��������
    /// </summary>
    /// <param name="signal"></param>
    private void StartParagraph(ParagraphStartedSignal signal) {
        ParagraphType type = signal.ParagraphDescription.CurrentType;
        if (_chapterMenuDialog != null) _chapterMenuDialog.Hide();
        switch (type) {
            case ParagraphType.Theoretical:
                ShowTheoreticalPart(signal.ParagraphDescription);
                break;
            case ParagraphType.Practical:
                ShowPlacticalPart();
                break;
            case ParagraphType.Verification:
                ShowVerificationPart();
                break;
            default:
                break;
        }
    }

    /// <summary>
    /// ����� ����� ���������
    /// </summary>
    /// <param name="chapterData"></param>
    private void ChapterPartFinish(ChapterPartFinishSignal signal) {
        // ���������� ��������� �������� �����
        _chapterMenuDialog.ShowProgressInPanel(signal.ChapterPartDescription.CurrentType);

        // ���������� ����� ��������

        //_learningProgressView
        // ���������� ����� ��������

        // ������� � ��������� �����
        CurrentExpValue += signal.ChapterPartDescription.ExpAmountToComplete;
        int _currentProgress = (CurrentExpValue / _fullExpAmount) * 100;

        _eventBus.Invoke(new LearningProgressChangedSignal(_currentProgress));
    }
    #endregion

    /// <summary>
    /// ����� "��������" �������������
    /// </summary>
    public void StopGame() {
        _eventBus.Invoke(new LearningModeStopSignal());
    }

    public void Dispose() {
        _eventBus.Unsubscribe<ChapterFinishedSignal>(ChapterFinished);
        _eventBus.Unsubscribe<ChapterSelectSignal>(SetChapter);
        _eventBus.Unsubscribe<ParagraphStartedSignal>(StartParagraph);
        _eventBus.Unsubscribe<ChapterPartFinishSignal>(ChapterPartFinish);
    }
}
