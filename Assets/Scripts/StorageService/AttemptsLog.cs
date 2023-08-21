using System.Collections.Generic;
using UnityEngine;
using CustomEventBus;
using CustomEventBus.Signals;

public class AttemptsLog : IService, IDisposable {
    private SavesManager _savesManager;
    private IStorageService _storageService;
    private List<AttemptInfo> _loadResults;

    private const string key = "attemptLog_save";
    private EventBus _eventBus;

    public void Init(SavesManager savesManager, EventBus eventBus) {
        _savesManager = savesManager;
        _storageService = _savesManager.CurrentService;
        _eventBus = eventBus;
        _eventBus.Subscribe((TaskCheckingFinishedSignal signal) => ExportAttemptInfo(signal.GeneralSwitchingResult));
    }

    public void ExportAttemptInfo(GeneralSwitchingResult generalSwitchingResult) {
        _loadResults = ImportAttemptInfo();                         // Получаем ранее сохранённые данные

        AttemptInfo attemptInfo = new AttemptInfo();
        attemptInfo.GetAttemptInfo(generalSwitchingResult);
        _loadResults.Add(attemptInfo);                                      // Дополняем сохранённые данные новыми                                        

        _storageService.Save(key, _loadResults);                            // Сохраняем данные
    }

    public List<AttemptInfo> ImportAttemptInfo() {
        List<AttemptInfo> list = new List<AttemptInfo>();
        _storageService.Load<List<AttemptInfo>>(key, loadResult => {        // Получаем сохранённые данные
            if (loadResult != null) list = loadResult;
            else list = new List<AttemptInfo>();
        });
        return list;
    }

    public string GetCorrectSwitchingCount(string taskID) {
        List<AttemptInfo> attempts = ImportAttemptInfo();
        if (attempts.Count == 0) return "-";

        var taskIdAttemptsList = attempts.FindAll(x => x.TaskID == taskID);
        var trueAttempts = taskIdAttemptsList.FindAll(x => x.CheckResult == true);
        if (trueAttempts.Count > 0) return $"{trueAttempts.Count}/{taskIdAttemptsList.Count}";
        else return $"0/{taskIdAttemptsList.Count}";
    }

    public TaskStatus GetStatus(string taskID) {
        List<AttemptInfo> attempts = ImportAttemptInfo();
        if (attempts.Count == 0) return TaskStatus.Lock;

        var taskIdAttemptsList = attempts.FindAll(x => x.TaskID == taskID);
        var trueAttempts = taskIdAttemptsList.FindAll(x => x.CheckResult == true);
        if (trueAttempts.Count > 0) return TaskStatus.Complite;
        else return TaskStatus.Unlock;
    }

    public string GetBestTime(string taskID) {
        List<AttemptInfo> attempts = ImportAttemptInfo();
        if (attempts.Count == 0) return "-";

        var taskIdAttemptsList = attempts.FindAll(x => x.TaskID == taskID);
        if (taskIdAttemptsList.Count == 0) return "-";

        float bestTimeValue = 999f;
        foreach (AttemptInfo iInfo in taskIdAttemptsList) {
            float iTime = iInfo.BuildTime;
            if (iTime < bestTimeValue) {
                bestTimeValue = iTime;
            }
        }

        return $"{GetFormattedTime(bestTimeValue)}";
    }

    private string GetFormattedTime(float _timeValue) {
        int minutes = Mathf.FloorToInt(_timeValue / 60f);
        int seconds = Mathf.FloorToInt(_timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_timeValue * 10f) % 10f);
        //int milliseconds = Mathf.FloorToInt((_timeValue * 1000f) % 1000f);

        if (minutes < 1) {
            return $"{seconds}.{milliseconds}";
        }
        else {
            if (minutes < 10) return $"0{minutes}:{seconds}.{milliseconds}";
            else return $"{minutes}:{seconds}.{milliseconds}";
        }
        //return string.Format("{0:00}:{1:00}.{2:0}", minutes, seconds, milliseconds);
    }

    public void Dispose() {
        _eventBus.Unsubscribe((TaskCheckingFinishedSignal signal) => ExportAttemptInfo(signal.GeneralSwitchingResult));
    }
}

