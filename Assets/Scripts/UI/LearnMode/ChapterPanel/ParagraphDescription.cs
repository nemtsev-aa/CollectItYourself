using CustomEventBus;
using CustomEventBus.Signals;
using System;
using UnityEngine;

public enum ParagraphType {
    Theoretical,
    Practical,
    Verification
}

/// <summary>
/// Модуль "Обучение": описание параграфа
/// </summary>
[CreateAssetMenu(fileName = nameof(ParagraphDescription), menuName = "LearningMode/" + nameof(ParagraphDescription))]
public class ParagraphDescription : ScriptableObject {
    [field: SerializeField] public ParagraphType CurrentType { get; private set; }
    [field: SerializeField] public string Title { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public int ExpAmountToComplete { get; private set; }
    [field: SerializeField] public int ExpAmountToUnlock { get; private set; }

    public int CurrentExpAmount => _currentExpAmount;
    
    public ChapterDescription ChapterParent => _chapterParent;

    public TaskStatus Status {
        get { return GetStatus(); }
    }

    private TaskStatus GetStatus() {
        if (_currentExpAmount < ExpAmountToUnlock) {
            return TaskStatus.Lock;
        } else if (_currentExpAmount >= ExpAmountToComplete) {
            return TaskStatus.Complite;
        } else {
            return TaskStatus.Unlock;
        }
    }

    [SerializeField] private int _currentExpAmount;
    private ChapterDescription _chapterParent;
    private TaskStatus _status;

    public void Init(ChapterDescription chapterParent) {
        _chapterParent = chapterParent;
    }

    public void AddExperience(int expValue) {
        _currentExpAmount += expValue;
        if (_currentExpAmount >= ExpAmountToComplete) {
            _status = TaskStatus.Complite;
            ServiceLocator.Current.Get<EventBus>().Invoke(new ChapterPartFinishSignal(this));
        }
    }
}
