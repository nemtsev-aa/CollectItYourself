using System.Collections.Generic;
using UnityEngine; 

[System.Serializable]
[CreateAssetMenu(fileName = nameof(SingleSwitchingResult), menuName = nameof(SingleSwitchingResult))]
public class SingleSwitchingResult : ScriptableObject {

    public string TaskName { get { return _taskName; } private set { _taskName = value; } }
    public bool CheckStatus { get { return _checkStatus; } private set { _checkStatus = value; } }
    public int SwitchBoxNumer { get { return _switchBoxNumer; } private set { _switchBoxNumer = value; } }
    public string ErrorCountText { get { return _errorCountText; } private set { _errorCountText = value; } }
    public float SwitchingTimeValue { get { return _switchingTimeValue; } private set { _switchingTimeValue = value; } }
    public List<ConnectionData> ErrorList { get { return _errorList; } private set { _errorList = value; } }
    
    // Конструктор класса
    public SingleSwitchingResult(string taskName, bool checkStatus, int switchBoxNumer, string errorCountText,
                                  float switchingTimeValue, List<ConnectionData> errorList) {
        _taskName = taskName;
        _checkStatus = checkStatus;
        _switchBoxNumer = switchBoxNumer;
        _errorCountText = errorCountText;
        _switchingTimeValue = switchingTimeValue;
        _errorList = errorList;
    }

    // Метод создания экземпляра класса
    public static SingleSwitchingResult CreateInstance(string taskName, bool checkStatus, int switchBoxNumer, string errorCountText,
                                  float switchingTimeValue, List<ConnectionData> errorList) {
        SingleSwitchingResult instance = CreateInstance<SingleSwitchingResult>();
        instance.TaskName = taskName;
        instance.CheckStatus = checkStatus;
        instance.SwitchBoxNumer = switchBoxNumer;
        instance.ErrorCountText = errorCountText;
        instance.SwitchingTimeValue = switchingTimeValue;
        instance.ErrorList = errorList;
        return instance;
    }

    private string _taskName;
    private bool _checkStatus;
    private int _switchBoxNumer;
    private string _errorCountText;
    private string _switchingTimeText;
    private float _switchingTimeValue;
    private List<ConnectionData> _errorList;

    public void SetSwitchingTimeValue(float value) {
        _switchingTimeValue = value;
    }

    public string GetSwitchingTimesText() {
        int minutes = Mathf.FloorToInt(_switchingTimeValue / 60f);
        int seconds = Mathf.FloorToInt(_switchingTimeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_switchingTimeValue * 1000f) % 1000f);

        return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
    }
}
