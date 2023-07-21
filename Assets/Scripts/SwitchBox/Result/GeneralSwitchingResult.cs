using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = nameof(GeneralSwitchingResult), menuName = nameof(GeneralSwitchingResult))]
public class GeneralSwitchingResult : ScriptableObject {
    public string CurrentDate { get { return _currentDate; } private set { _currentDate = value; } }
    public TaskData TaskData { get { return _taskData; } private set { _taskData = value; } }
    public bool CheckResult { get { return _checkResult; } private set { _checkResult = value; } }
    public int SwitchBoxNumber { get { return _switchBoxNumber; } private set { _switchBoxNumber = value; } }
    public List<SingleSwitchingResult> SingleSwichingResults { get { return _singleSwichingResults; } private set { _singleSwichingResults = value; } }
    public string ErrorsCountText { get { return _errorsCountText; } private set { _errorsCountText = value; } }
    public float SwitchingTimesValue { get { return _switchingTimesValue; } private set { _switchingTimesValue = value; } }
    public List<ConnectionData> ErrorsList { get { return _errorsList; } private set { _errorsList = value; } }

    [SerializeField] private string _currentDate;
    [SerializeField] private TaskData _taskData;
    [SerializeField] private bool _checkResult;
    [SerializeField] private int _switchBoxNumber;
    [SerializeField] private List<SingleSwitchingResult> _singleSwichingResults;
    [SerializeField] private string _errorsCountText;
    [SerializeField] private float _switchingTimesValue;
    [SerializeField] private List<ConnectionData> _errorsList;

    // Конструктор класса
    public GeneralSwitchingResult(string currentDate, TaskData taskData, bool checkResult, int switchBoxNumber, List<SingleSwitchingResult> singleSwichingResults,
                                  string errorsCountText, float switchingTimesValue, List<ConnectionData> errorsList) {
        
        _currentDate = currentDate;
        _taskData = taskData;
        _checkResult = checkResult;
        _switchBoxNumber = switchBoxNumber;
        _singleSwichingResults = singleSwichingResults;
        _errorsCountText = errorsCountText;
        _switchingTimesValue = switchingTimesValue;
        _errorsList = errorsList;
    }

    // Метод создания экземпляра класса
    public static GeneralSwitchingResult CreateInstance(string currentDate, TaskData taskData, bool checkResult, int switchBoxNumber, 
                                                        List<SingleSwitchingResult> singleSwichingResults,
                                                        string errorsCountText, float switchingTimesValue,
                                                        List<ConnectionData> errorsList) {
        GeneralSwitchingResult instance = CreateInstance<GeneralSwitchingResult>();
        instance.CurrentDate = currentDate;
        instance.TaskData = taskData;
        instance.CheckResult = checkResult;
        instance.SwitchBoxNumber = switchBoxNumber;
        instance.SingleSwichingResults = singleSwichingResults;
        instance.ErrorsCountText = errorsCountText;
        instance.SwitchingTimesValue = switchingTimesValue;
        instance.ErrorsList = errorsList;

#if UNITY_EDITOR
        // Создаем путь для сохранения ScriptableObject
        string path = $"Assets/Resources/Task/Result_{taskData.ID}.asset";
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
                timesValue += iResult.SwitchingTimeValue;
            }
            return timesValue;
        }
        return 0f;
    }

    public string GetSwitchingTimesText() {
        string switchingTimesText = "";
        if (_singleSwichingResults.Count() > 0) {
            float timesValue = 0f;
            foreach (SingleSwitchingResult iResult in _singleSwichingResults) {
                timesValue += iResult.SwitchingTimeValue;
            }
            switchingTimesText = GetFormattedTime(timesValue);
            return switchingTimesText;
        }
        return null;
    }

    private string GetFormattedTime(float _timeValue) {
        int minutes = Mathf.FloorToInt(_timeValue / 60f);
        int seconds = Mathf.FloorToInt(_timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_timeValue * 1000f) % 1000f);

        return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
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
