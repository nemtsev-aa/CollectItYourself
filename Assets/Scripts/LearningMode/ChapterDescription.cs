using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChapterType {
    Chapter1,
    Chapter2,
    Chapter3,
    Chapter4
}

[CreateAssetMenu(fileName = nameof(ChapterDescription), menuName = "LearningMode/" + nameof(ChapterDescription))]
[System.Serializable]
/// <summary>
/// Описание главы в режиме "Обучение"
/// </summary>
public class ChapterDescription : Description {
    public ChapterType Type;
    public IEnumerable<ParagraphDescription> ParagraphDescriptions => _paragraphDescription;
    [SerializeField] private List<ParagraphDescription> _paragraphDescription = new List<ParagraphDescription>();
    
    private void OnValidate() {
        foreach (var item in _paragraphDescription) {
            item.Init(this);
        }
    }

    /// <summary>
    /// Значение прогресса
    /// </summary>
    public int ProgressValue {
        get {
            return CurrentExpAmount / ExpAmountToComplete;
        }
    }

    /// <summary>
    /// Количество опыта для завершения
    /// </summary>
    public int ExpAmountToComplete { 
        get {
            return GetExpAmounthToComplite();
            }
        private set {
            ExpAmountToComplete = value;
            } 
    }

    private int GetExpAmounthToComplite() {
        if (_paragraphDescription.Count == 0) {
            Debug.LogError($"В главу {this.name} не добавлены параграфы!");
            return 0;
        } 

        int summ = 0;
        foreach (var iDescription in _paragraphDescription) {
            summ += iDescription.ExpAmountToComplete;
        }
        return summ;
    }

    /// <summary>
    /// Количество опыта для разблокирования
    /// </summary>
    public int ExpAmountToUnlock {
        get {
            return GetExpAmountToUnlock();
        }
        private set {
            ExpAmountToUnlock = value;
        }
    }

    private int GetExpAmountToUnlock() {
        if (_paragraphDescription.Count == 0) {
            Debug.LogError($"В главу {this.name} не добавлены параграфы!");
            return 0;
        }

        int summ = 0;
        foreach (var iDescription in _paragraphDescription) {
            summ += iDescription.ExpAmountToUnlock;
        }
        return summ;
    }

    /// <summary>
    /// Количество полученного опыта
    /// </summary>
    public int CurrentExpAmount {
        get {
            return GetCurrentExpAmount();
        }
        private set {
            CurrentExpAmount = value;
        }
    }

    private int GetCurrentExpAmount() {
        if (_paragraphDescription.Count == 0) {
            Debug.LogError($"В главу {this.name} не добавлены параграфы!");
            return 0;
        }

        int summ = 0;
        foreach (var iDescription in _paragraphDescription) {
            summ += iDescription.CurrentExpAmount;
        }
        return summ;
    }
}
