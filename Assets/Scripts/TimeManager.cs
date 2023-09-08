using CustomEventBus;
using CustomEventBus.Signals;
using System;
using UnityEngine;

public enum TimeManagerMode {
    Stopwatch,
    Timer
}

public class TimeManager : MonoBehaviour, IService, CustomEventBus.IDisposable {
    [field: SerializeField] public bool Status { get; private set; }
    public TimeManagerMode CurrentMode => _currentMode;

    [SerializeField] private TimeManagerMode _currentMode = TimeManagerMode.Stopwatch;
    [SerializeField] private TimeView _timeView;
    [SerializeField] private float _timeValue;

    public Action<string> TimeChanged;
    public Action<bool> StatusChanged;
    
    private EventBus _eventBus;

    public void Init(EventBus eventBus) {
        _eventBus = eventBus;
        _eventBus.Subscribe((TrainingModeStopSignal signal) => SetStatus(false));
        _eventBus.Subscribe((TrainingModeStartSignal signal) => SetStatus(true));
    }

    public void SetTimeView(TimeView timeView) {
        _timeView = timeView;
        _timeView.Init(this);

        TimeChanged += _timeView.ShowTimeValue;
        StatusChanged += _timeView.ShowStatus;
    }

    private void Update() {
        if (Status) {
            _timeValue += Time.deltaTime;
            TimeChanged?.Invoke(GetFormattedTime());
        }
    }

    public void SetStatus(bool status) {
        Status = status;
        StatusChanged?.Invoke(Status);
    }

    public void Stop() {
        _timeValue = 0;
        StatusChanged?.Invoke(Status);
        TimeChanged?.Invoke(GetFormattedTime());
    }

    public void ResetTimer() {
        _timeValue = 0f;

        if (_timeView != null) {
            TimeChanged -= _timeView.ShowTimeValue;
            StatusChanged -= _timeView.ShowStatus;
            _timeView = null;
        }
    }

    private string GetFormattedTime() {
        int minutes = Mathf.FloorToInt(_timeValue / 60f);
        int seconds = Mathf.FloorToInt(_timeValue % 60f);
        int milliseconds = Mathf.FloorToInt((_timeValue * 10f) % 10f);
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

    public void SetTimeValue(float timeValue) {
        _timeValue = timeValue;
    }

    public float GetTimeValue() {
        return _timeValue;
    }

    public string GetTimeText() {
        return GetFormattedTime();
    }

    public void Dispose() {
        _timeView = null;
        _eventBus.Unsubscribe((TrainingModeStopSignal signal) => SetStatus(false));
        _eventBus.Unsubscribe((TrainingModeStartSignal signal) => SetStatus(true));
    }
}
