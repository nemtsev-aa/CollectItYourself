using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Класс для сбора статистики по каждому заданию
/// </summary>
[CreateAssetMenu(fileName = nameof(TaskStatistics), menuName = "Tasks/ScriptableObjects/" + nameof(TaskStatistics))]
[System.Serializable]
public class TaskStatistics : ScriptableObject {
    public string ID => _id;
    public IEnumerable<GeneralSwitchingResult> Attempts => _attempts;
    
    /// <summary>
    /// Уникальный ID задания
    /// </summary>
    [SerializeField] private string _id;
    /// <summary>
    /// Список попыток
    /// </summary>
    [SerializeField] private List<GeneralSwitchingResult> _attempts;



    public string GetCorrectSwitchingCount() {
        string correctSwitchingCount = "";
        string attemptsCount = _attempts.Count.ToString();

        if (_attempts.Count > 0) {
            int correctCount = 0;
            foreach (GeneralSwitchingResult iResult in _attempts) {
                if (iResult.CheckResult) {
                    correctCount++;
                }
            }
            correctSwitchingCount = correctCount.ToString();
        } else {
            return "-/-";
        }
        return $"{correctSwitchingCount}/{attemptsCount}";
    }

    public string GetBestTime() {
        string bestTime = "";
        float bestTimeValue = 999f;
        GeneralSwitchingResult bestResult = null;

        if (_attempts.Count > 0) {
            foreach (GeneralSwitchingResult iResult in _attempts) {
                float iTime = iResult.GetSwitchingTimesValue();
                if (iTime < bestTimeValue) {
                    bestResult = iResult;
                }
            }
            bestTime = bestResult.GetSwitchingTimesText();
        }
        else {
            return "-";
        }
        return $"{bestTime}";
    }

    public bool AddAttempt(GeneralSwitchingResult result) {
        if (!_attempts.Contains(result)) { 
            _attempts.Add(result);
            return true;
        }
        return false;
    }
}
