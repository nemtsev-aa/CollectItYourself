using CustomEventBus;
using CustomEventBus.Signals;
using System;
using System.Collections;
using System.Collections.Generic;
using UI.Dialogs;
using UnityEngine;
/// <summary>
/// �������� �� ������ ������ "��������":
/// ����������� ����� 
/// ���������� ��������� ������� ��� ������� ����� ����������
/// ���������� ��� ������ �������
/// </summary>
public class LearningProgressManager : IService, CustomEventBus.IDisposable {
    [SerializeField] private LearningProgressView _learningProgressView;
    [field: SerializeField] public int CurrentExpValue { get; private set; }
    
    private int _fullExpAmount;
    private EventBus _eventBus;

    [ContextMenu("Initialization")]
    public void Initialization() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ChapterSelectSignal>(SetChapter);
        _eventBus.Subscribe<ChapterFinishedSignal>(ChapterFinished);
    }

    /// <summary>
    /// ����� �������
    /// </summary>
    /// <param name="signal"></param>
    private void SetChapter(ChapterSelectSignal signal) {
        _eventBus.Invoke(new ChapterSelectSignal(signal.ChapterData));
    }

    /// <summary>
    /// ����� ���������
    /// </summary>
    /// <param name="signal"></param>
    private void ChapterFinished(ChapterFinishedSignal signal) {
        var chapterData = signal.ChapterData;

        StopGame();

        // ���������� ������ ����� ��������
        ChapterComplite(signal.ChapterData);
        ChaptersMenuDialog captersMenuDialog = DialogManager.ShowDialog<ChaptersMenuDialog>();
    }

    /// <summary>
    /// ����� "��������" �������������
    /// </summary>
    public void StopGame() {
        _eventBus.Invoke(new LearningModeStopSignal());
    }

    /// <summary>
    /// ����� ���������
    /// </summary>
    /// <param name="chapterData"></param>
    private void ChapterComplite(ChapterData chapterData) {
        CurrentExpValue += chapterData.ExpAmountToComplete;
        int _currentProgress = (CurrentExpValue / _fullExpAmount) * 100;
        _eventBus.Invoke(new LearningProgressChangedSignal(_currentProgress));
    }

    public void Dispose() {
        _eventBus.Unsubscribe<ChapterFinishedSignal>(ChapterFinished);
        _eventBus.Unsubscribe<ChapterSelectSignal>(SetChapter);
    }
}
