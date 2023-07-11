using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = nameof(GeneralSwitchingResult), menuName = nameof(GeneralSwitchingResult))]
public class GeneralSwitchingResult : ScriptableObject {
    private TaskData _taskData;
    private bool _checkResult;
    private int _switchBoxNumber;
    private List<SingleSwitchingResult> _singleSwichingResults;
    private string _errorsCountText;
    private string _switchingTimesText;
    private float _switchingTimesValue;
    private List<ConnectionData> _errorsList;

    public TaskData TaskData => _taskData;
    public bool CheckResult => _checkResult;
    public int SwitchBoxNumber => _switchBoxNumber;
    public List<SingleSwitchingResult> SingleSwichingResults => _singleSwichingResults;
    public string ErrorsCountText => _errorsCountText;
    public string SwitchingTimesText => _switchingTimesText;
    public float SwitchingTimesValue => _switchingTimesValue;
    public List<ConnectionData> ErrorsList => _errorsList;

    public string GetErrorsCountText() {
        if (_singleSwichingResults.Count > 0) {
            int errorsCountValue = 0;
            int contactsCountValue = 0;

            foreach (SingleSwitchingResult iResult in _singleSwichingResults) {
                string[] iData = iResult.ErrorCountText.Split("/");
                int iErrorsCount = int.Parse(iData[0]);
                int iContactsCount = int.Parse(iData[1]);

                errorsCountValue += iErrorsCount;
                contactsCountValue += iContactsCount;
            }

            _errorsCountText = errorsCountValue + "/" + contactsCountValue;
            return _errorsCountText;
        }
        return null;
    }

    public float GetSwitchingTimesValue() {
        if (_singleSwichingResults.Count > 0) {
            float timesValue = 0f;
            foreach (SingleSwitchingResult iResult in _singleSwichingResults) {
                timesValue += iResult.SwitchingTimeValue;
            }
            return timesValue;
        }
        return 0f;
    }

    public string GetSwitchingTimesText() {
        if (_singleSwichingResults.Count > 0) {
            float timesValue = 0f;
            foreach (SingleSwitchingResult iResult in _singleSwichingResults) {
                timesValue += iResult.SwitchingTimeValue;
            }
            _switchingTimesText = GetFormattedTime(timesValue);
            return _switchingTimesText;
        }
        return null;
    }

    private string GetFormattedTime(float _timeValue) {
        int minutes = Mathf.FloorToInt(_timeValue / 60f);
        int seconds = Mathf.FloorToInt(_timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_timeValue * 1000f) % 1000f);

        return string.Format("{0:00}:{1:00}:{2:000}", minutes, seconds, milliseconds);
    }

    public List<ConnectionData> GetErrorsList() {
        if (_singleSwichingResults.Count > 0) {
            foreach (SingleSwitchingResult iResult in _singleSwichingResults) {
                _errorsList.AddRange(iResult.ErrorList);
            }
            return _errorsList;
        }
        return null;
    }
}
