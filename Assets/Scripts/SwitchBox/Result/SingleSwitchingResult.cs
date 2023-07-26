using System;
using System.Collections.Generic;
using UnityEditor;
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
    
    [SerializeField] private string _taskName;
    [SerializeField] private bool _checkStatus;
    [SerializeField] private int _switchBoxNumer;
    [SerializeField] private string _errorCountText;
    [SerializeField] private string _switchingTimeText;
    [SerializeField] private float _switchingTimeValue;
    [SerializeField] private List<ConnectionData> _errorList;

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

#if UNITY_EDITOR
        // Создаем путь для сохранения ScriptableObject
        string formattedTime = DateTime.Now.ToString("HHmmss");
        string path = $"Assets/Resources/Task/SingleResult_{taskName}_{formattedTime}.asset";
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

    public void SetSwitchingTimeValue(float value) {
        _switchingTimeValue = value;
    }

    public string GetSwitchingTimesText() {
        int minutes = Mathf.FloorToInt(_switchingTimeValue / 60f);
        int seconds = Mathf.FloorToInt(_switchingTimeValue % 60f);
        //int milliseconds = Mathf.FloorToInt((_switchingTimeValue * 1000f) % 1000f);
        
        int milliseconds = Mathf.FloorToInt((_switchingTimeValue * 10f) % 10f);
        //int milliseconds = Mathf.FloorToInt((_timeValue * 1000f) % 1000f);

        if (minutes < 1) {
            return $"{seconds}.{milliseconds}";
        }
        else {
            if (minutes < 10) {
                return $"0{minutes}:{seconds}.{milliseconds}";
            } else {
                return $"{minutes}:{seconds}.{milliseconds}";
            }
        }
        //return string.Format("{0:00}:{1:00}:{2:0}", minutes, seconds, milliseconds);
    }
}
