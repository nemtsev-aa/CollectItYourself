using CustomEventBus;
using CustomEventBus.Signals;
using System;
using UI.Dialogs;
using UnityEngine;

/// <summary>
/// Отвечает за логику модуля "Обучение":
/// Переключает главы 
/// Уведомляет остальные системы что текущая глава изменилась
/// Уведомляет что модуль пройден
/// </summary>
public class LearningModeManager : IService, CustomEventBus.IDisposable {
    [field: SerializeField] public int CurrentExpValue { get; private set; }
    private LearningProgressView _learningProgressView;                                             // Виджет прогресса в модуле "Обучение"
    
    private LearningModeMenuDialog _learningModeMenuDialog;                                         // Окно выбора главы в модуле "Обучение"
    private TheoreticalPartDialog _theoreticalPartDialog;                                           // Окно теоретической части в модуле "Обучение"
    private PracticalPartDialog _practicalPartDialog;                                               // Окно практической части в модуле "Обучение"
    private VerificationPartDialog _verificationPartDialog;                                         // Окно проверочной части в модуле "Обучение"

    private int _fullExpAmount;
    private EventBus _eventBus;
 
    [ContextMenu("Init")]
    public void Init() {
        _eventBus = ServiceLocator.Current.Get<EventBus>();
        _eventBus.Subscribe<ChapterSelectSignal>(SetChapter);
        _eventBus.Subscribe<ChapterFinishedSignal>(ChapterFinished);
    }

    public void ShowLearningModeMenu() {
        _learningModeMenuDialog = DialogManager.ShowDialog<LearningModeMenuDialog>();
        _learningModeMenuDialog.Init(this);
    }

    public void ReturnToLearningModeMenu() {
        _theoreticalPartDialog.Hide();
    }

    public void ShowPlacticalPart() {
        _theoreticalPartDialog.Hide();
        _practicalPartDialog = DialogManager.ShowDialog<PracticalPartDialog>();
        _practicalPartDialog.Init(this);
    }

    /// <summary>
    /// Глава выбрана
    /// </summary>
    /// <param name="signal"></param>
    private void SetChapter(ChapterSelectSignal signal) {
        _theoreticalPartDialog = DialogManager.ShowDialog<TheoreticalPartDialog>();
        _theoreticalPartDialog.Init(signal.ChapterDescription, this);
    }

    /// <summary>
    /// Глава завершена
    /// </summary>
    /// <param name="signal"></param>
    private void ChapterFinished(ChapterFinishedSignal signal) {
        var chapterData = signal.ChapterDescription;

        StopGame();

        // Показываем виджкт очков обучения
        ChapterComplite(signal.ChapterDescription);
        LearningModeMenuDialog captersMenuDialog = DialogManager.ShowDialog<LearningModeMenuDialog>();
    }

    /// <summary>
    /// Режим "Обучение" преостановлен
    /// </summary>
    public void StopGame() {
        _eventBus.Invoke(new LearningModeStopSignal());
    }

    /// <summary>
    /// Глава завершена
    /// </summary>
    /// <param name="chapterData"></param>
    private void ChapterComplite(LearningModeDescription chapterData) {
        CurrentExpValue += chapterData.ExpAmountToComplete;
        int _currentProgress = (CurrentExpValue / _fullExpAmount) * 100;
        _eventBus.Invoke(new LearningProgressChangedSignal(_currentProgress));
    }

    public void Dispose() {
        _eventBus.Unsubscribe<ChapterFinishedSignal>(ChapterFinished);
        _eventBus.Unsubscribe<ChapterSelectSignal>(SetChapter);
    }
}
