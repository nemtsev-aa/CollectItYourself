using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = nameof(GeneralSwitchingResult), menuName = nameof(GeneralSwitchingResult))]
public class GeneralSwitchingResult : ScriptableObject {
    public TaskData TaskData { get { return _taskData; } private set { _taskData = value; } }
    public bool CheckResult { get { return _checkResult; } private set { _checkResult = value; } }
    public int SwitchBoxNumber { get { return _switchBoxNumber; } private set { _switchBoxNumber = value; } }
    public IEnumerable<SingleSwitchingResult> SingleSwichingResults { get { return _singleSwichingResults; } private set { _singleSwichingResults = value; } }
    public string ErrorsCountText { get { return _errorsCountText; } private set { _errorsCountText = value; } }
    public float SwitchingTimesValue { get { return _switchingTimesValue; } private set { _switchingTimesValue = value; } }
    public List<ConnectionData> ErrorsList { get { return _errorsList; } private set { _errorsList = value; } }

    private TaskData _taskData;
    private bool _checkResult;
    private int _switchBoxNumber;
    private IEnumerable<SingleSwitchingResult> _singleSwichingResults;
    private string _errorsCountText;
    private float _switchingTimesValue;
    private List<ConnectionData> _errorsList;

    // Конструктор класса
    public GeneralSwitchingResult(TaskData taskData, bool checkResult, int switchBoxNumber, IEnumerable<SingleSwitchingResult> singleSwichingResults,
                                  string errorsCountText, float switchingTimesValue, List<ConnectionData> errorsList) {
        _taskData = taskData;
        _checkResult = checkResult;
        _switchBoxNumber = switchBoxNumber;
        _singleSwichingResults = singleSwichingResults;
        _errorsCountText = errorsCountText;
        _switchingTimesValue = switchingTimesValue;
        _errorsList = errorsList;
    }

    // Метод создания экземпляра класса
    public static GeneralSwitchingResult CreateInstance(TaskData taskData, bool checkResult, int switchBoxNumber, 
                                                        IEnumerable<SingleSwitchingResult> singleSwichingResults,
                                                        string errorsCountText, float switchingTimesValue,
                                                        List<ConnectionData> errorsList) {
        GeneralSwitchingResult instance = CreateInstance<GeneralSwitchingResult>();
        instance.TaskData = taskData;
        instance.CheckResult = checkResult;
        instance.SwitchBoxNumber = switchBoxNumber;
        instance.SingleSwichingResults = singleSwichingResults;
        instance.ErrorsCountText = errorsCountText;
        instance.SwitchingTimesValue = switchingTimesValue;
        instance.ErrorsList = errorsList;

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
