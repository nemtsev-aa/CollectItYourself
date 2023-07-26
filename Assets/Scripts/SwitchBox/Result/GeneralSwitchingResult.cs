using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = nameof(GeneralSwitchingResult), menuName = nameof(GeneralSwitchingResult))]
public class GeneralSwitchingResult : ScriptableObject {
    public string CurrentDate { get { return _currentDate; } private set { _currentDate = value; } }
    public TaskData TaskData { get { return _taskData; } private set { _taskData = value; } }
    public bool CheckStatus { get { return _checkStatus; } private set { _checkStatus = value; } }
    public int SwitchBoxNumber { get { return _switchBoxNumber; } private set { _switchBoxNumber = value; } }
    public List<SingleSwitchingResult> SingleSwichingResults { get { return _singleSwichingResults; } private set { _singleSwichingResults = value; } }
    public string ErrorsCountText { get { return _errorsCountText; } private set { _errorsCountText = value; } }
    public float SwitchingTimesValue { get { return _switchingTimesValue; } private set { _switchingTimesValue = value; } }
    public List<ConnectionData> ErrorsList { get { return _errorsList; } private set { _errorsList = value; } }

    [SerializeField] private string _currentDate;
    [SerializeField] private TaskData _taskData;
    [SerializeField] private bool _checkStatus;
    [SerializeField] private int _switchBoxNumber;
    [SerializeField] private List<SingleSwitchingResult> _singleSwichingResults;
    [SerializeField] private string _errorsCountText;
    [SerializeField] private float _switchingTimesValue;
    [SerializeField] private List<ConnectionData> _errorsList;

    // Конструктор класса
    public GeneralSwitchingResult(string currentDate, TaskData taskData, bool checkStatus, int switchBoxNumber, List<SingleSwitchingResult> singleSwichingResults,
                                  string errorsCountText, float switchingTimesValue, List<ConnectionData> errorsList) {
        
        _currentDate = currentDate;
        _taskData = taskData;
        _checkStatus = checkStatus;
        _switchBoxNumber = switchBoxNumber;
        _singleSwichingResults = singleSwichingResults;
        _errorsCountText = errorsCountText;
        _switchingTimesValue = switchingTimesValue;
        _errorsList = errorsList;
    }

    // Метод создания экземпляра класса
    public static GeneralSwitchingResult CreateInstance(string currentDate, TaskData taskData, bool checkStatus, int switchBoxNumber, 
                                                        List<SingleSwitchingResult> singleSwichingResults,
                                                        string errorsCountText, float switchingTimesValue,
                                                        List<ConnectionData> errorsList) {
        GeneralSwitchingResult instance = CreateInstance<GeneralSwitchingResult>();
        instance.CurrentDate = currentDate;
        instance.TaskData = taskData;
        instance.CheckStatus = checkStatus;
        instance.SwitchBoxNumber = switchBoxNumber;
        instance.SingleSwichingResults = singleSwichingResults;
        instance.ErrorsCountText = errorsCountText;
        instance.SwitchingTimesValue = switchingTimesValue;
        instance.ErrorsList = errorsList;

#if UNITY_EDITOR
        // Создаем путь для сохранения ScriptableObject
        string formattedTime = DateTime.Now.ToString("HHmmss");
        string path = $"Assets/Resources/Task/Result_{taskData.ID}_{formattedTime}.asset";
        // Проверяем, существует ли файл по указанному пути
        if (AssetDatabase.IsValidFolder(path)) {
            // Если файл уже существует, удаляем его
            AssetDatabase.DeleteAsset(path);
        }
        // Создаем ресурс по указанному пути
        AssetDatabase.CreateAsset(instance, path);
        // Обновляем активную базу данных ресурсов
        AssetDatabase.Refresh();
#endif
        return instance;
    }

    public float GetSwitchingTimesValue() {
        if (_singleSwichingResults.Count() > 0) {
            float timesValue = 0f;
            foreach (SingleSwitchingResult iResult in _singleSwichingResults) {
                if (iResult != null) {
                    timesValue += iResult.SwitchingTimeValue;
                }
            }
            return timesValue;
        } else {
            return _switchingTimesValue;
        }
    }

    public string GetSwitchingTimesText() {
        if (_singleSwichingResults.Count() > 0) {
            float timesValue = 0f;
            foreach (SingleSwitchingResult iResult in _singleSwichingResults) {
                if (iResult != null) {
                    timesValue += iResult.SwitchingTimeValue;
                }            
            }
            return GetFormattedTime(timesValue);
        } else {
            return GetFormattedTime(_switchingTimesValue);
        }
    }

    private string GetFormattedTime(float _timeValue) {
        int minutes = Mathf.FloorToInt(_timeValue / 60f);
        int seconds = Mathf.FloorToInt(_timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_timeValue * 10f) % 10f);
        //int milliseconds = Mathf.FloorToInt((_timeValue * 1000f) % 1000f);

        if (minutes < 1) {
            return $"{seconds}.{milliseconds}";
        } else {
            if (minutes < 10) {
                return $"0{minutes}:{seconds}.{milliseconds}";
            } else {
                return $"{minutes}:{seconds}.{milliseconds}";
            }
        }
        //return string.Format("{0:00}:{1:00}.{2:0}", minutes, seconds, milliseconds);
    }

    public List<ConnectionData> GetErrorsList() {
        if (_singleSwichingResults.Count() > 0) {
            foreach (SingleSwitchingResult iResult in _singleSwichingResults) {
                _errorsList.AddRange(iResult.ErrorList);
            }
            return _errorsList;
        }
        return null;
    }
}
